using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PMAPortal.Web.Data.Repositories;
using PMAPortal.Web.DTOs;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services.Implementations
{
    public class InstallationBatchService: IInstallationBatchService
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IRepository<InstallationBatch> batchRepo;
        private readonly IRepository<InstallationBatchItem> batchItemRepo;
        private readonly IRepository<Customer> customerRepo;
        private readonly IRepository<Installation> installationRepo;
        private readonly IRepository<Survey> surveyRepo;
        private readonly IOptionsSnapshot<AppSettings> appSettingsDelegate;
        private readonly IHttpContextAccessor accessor;
        private readonly ILoggerService<BatchService> logger;
        private string[] headers = new string[] { "SN", "Date Shared", "Batch", "Account Number", "Customer Name" };

        public InstallationBatchService(
            IWebHostEnvironment hostEnvironment,
            IRepository<InstallationBatch> batchRepo,
            IRepository<InstallationBatchItem> batchItemRepo,
            IRepository<Customer> customerRepo,
            IRepository<Installation> installationRepo,
            IRepository<Survey> surveyRepo,
            IOptionsSnapshot<AppSettings> appSettingsDelegate,
            IHttpContextAccessor accessor,
            ILoggerService<BatchService> logger)
        {
            this.hostEnvironment = hostEnvironment;
            this.batchRepo = batchRepo;
            this.batchItemRepo = batchItemRepo;
            this.customerRepo = customerRepo;
            this.installationRepo = installationRepo;
            this.surveyRepo = surveyRepo;
            this.appSettingsDelegate = appSettingsDelegate;
            this.accessor = accessor;
            this.logger = logger;
        }

        private bool ValidateFile(IFormFile file, out List<string> errorItems)
        {
            bool isValid = true;
            List<string> errList = new List<string>();
            var maxUploadSize = appSettingsDelegate.Value.MaxUploadSize;
            if (file == null)
            {
                isValid = false;
                errList.Add("No file uploaded.");
            }
            else
            {
                if (file.Length > (maxUploadSize * 1024 * 1024))
                {
                    isValid = false;
                    errList.Add($"Max upload size exceeded. Max size is {maxUploadSize}MB");
                }
                var ext = Path.GetExtension(file.FileName);
                if (ext != ".xls" && ext != ".xlsx")
                {
                    isValid = false;
                    errList.Add($"Invalid file format. Supported file formats include .xls and .xlsx");
                }

                if (batchRepo.Any(b => b.FileName == file.FileName))
                {
                    isValid = false;
                    errList.Add($"A batch file with name '{file.FileName}' already exist");
                }
            }
            errorItems = errList;
            return isValid;
        }

        private bool ValidateHeader(DataRow row, string[] headers, out string errorMessage)
        {
            var err = "";
            var isValid = true;
            for (int i = 0; i < headers.Length; i++)
            {
                if (row[i] == null || Convert.ToString(row[i]).Trim().ToLower() != headers[i].ToLower())
                {
                    isValid = false;
                    err = $"Invalid header value at column {i + 1}. Expected value is {headers[i]}";
                    break;
                }
            }
            errorMessage = err;
            return isValid;
        }

        private bool ValidateDataRow(int index, DataRow row, out string errorMessage)
        {
            var err = "";
            var isValid = true;
            if (row[0] == null || Convert.ToString(row[0]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[0]} at row {index}. Field is required.";
            }
            else if (row[1] == null || !DateTimeOffset.TryParse(Convert.ToString(row[1]).Trim(), out _))
            {
                isValid = false;
                err = $"Invalid value for {headers[1]} at row {index}. A valid date in the format YYYY-MM-DD is required.";
            }
            else if (row[2] == null || Convert.ToString(row[2]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[2]} at row {index}. Field is required.";
            }
            else if (row[3] == null || Convert.ToString(row[3]).Trim().Length != 11)
            {
                isValid = false;
                err = $"Invalid value for {headers[3]} at row {index}. 11 character value expected.";
            }
            else if (row[4] == null || Convert.ToString(row[4]).Trim() == "")
            {
                isValid = false;
                err = $"Invalid value for {headers[4]} at row {index}. Field is required.";
            }
            

            errorMessage = err;
            return isValid;
        }

        private IEnumerable<InstallationBatchItem> ExtractData(IFormFile file)
        {
            List<InstallationBatchItem> batchItems = new List<InstallationBatchItem>();
            IExcelDataReader excelReader = null;
            DataSet dataSet = new DataSet();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var fileStream = file.OpenReadStream();

            if (file.FileName.EndsWith(".xls"))
                excelReader = ExcelReaderFactory.CreateBinaryReader(fileStream);
            else if (file.FileName.EndsWith(".xlsx"))
                excelReader = ExcelReaderFactory.CreateReader(fileStream);
            else
                throw new AppException($"Invalid file '{file.FileName}'");

            dataSet = excelReader.AsDataSet();
            excelReader.Close();

            if (dataSet == null || dataSet.Tables.Count == 0)
                throw new Exception($"Unable to read file. Ensure file complies with the specified template.");

            var table = dataSet.Tables[0];
            var header = table.Rows[0];
            if (!ValidateHeader(header, headers, out string error))
            {
                throw new AppException(error);
            }
            else
            {
                //validate and load data
                var rows = table.Rows;
                if (rows.Count <= 1)
                {
                    throw new AppException($"Excel is empty!");
                }
                for (int i = 1; i < rows.Count; i++)
                {
                    if (!ValidateDataRow(i, rows[i], out error))
                    {
                        throw new AppException(error);
                    }
                    else
                    {
                        var batchItem = new InstallationBatchItem()
                        {
                            SN = Convert.ToString(rows[i][0]),
                            DateShared = DateTimeOffset.Parse(Convert.ToString(rows[i][1])),
                            BatchNumber = Convert.ToString(rows[i][2]),
                            AccountNumber = Convert.ToString(rows[i][3]),
                            CustomerName = Convert.ToString(rows[i][4])
                        };

                        batchItems.Add(batchItem);
                    }
                }
            }
            fileStream.Dispose();
            return batchItems;
        }

        private string SaveFile(IFormFile file)
        {
            string uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "uploads/batches");
            var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            var _filePath = "uploads/batches/" + fileName;
            return _filePath;
        }

        private void DeleteFile(string filePath)
        {
            string fullPath = Path.Combine(hostEnvironment.WebRootPath, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public async Task AddInstallationBatch(IFormFile file, DateTimeOffset? dateShared = null)
        {
            if (file == null)
            {
                throw new AppException($"No file uploaded!");
            }
            else
            {
                if (!ValidateFile(file, out List<string> errItems))
                {
                    throw new AppException($"Invalid file uploaded", errorItems: errItems);
                }
                else
                {
                    var currentUser = accessor.HttpContext.GetUserSession();
                    var batchItems = ExtractData(file).ToList();

                    // check for duplicate account number
                    var duplicateAccsOnExcel = batchItems.GroupBy(c => c.AccountNumber).Where(g => g.Count() > 1).Select(g => g.Key);
                    if (duplicateAccsOnExcel.Count() > 0)
                    {
                        throw new AppException($"Duplicate account numbers detected: {string.Join(", ", duplicateAccsOnExcel)}");
                    }

                    var accs = batchItems.Select(c => c.AccountNumber);
                    var duplicateAccsInDb = batchItemRepo.GetWhere(c => accs.Contains(c.AccountNumber)).Select(c => c.AccountNumber);
                    if (duplicateAccsInDb.Count() > 0)
                    {
                        throw new AppException($"One or more accounts already exist/uploaded: {string.Join(", ", duplicateAccsInDb)}");
                    }

                    // check for invalid account numbers
                    //var invalidAccs = accs.Where(acc => !customerRepo.Any(c => c.AccountNumber == acc));
                    var validAccs = customerRepo.GetWhere(c => accs.Contains(c.AccountNumber)).Select(c => c.AccountNumber).ToList();
                    var invalidAccs = accs.Where(acc => !validAccs.Contains(acc));
                    if (invalidAccs.Count() > 0)
                    {
                        throw new AppException($"One or more accounts are invalid or does not exist in the system: {string.Join(", ", invalidAccs)}");
                    }

                    // check that customer with account is meter ready
                    var notMeterReady = accs.Where(acc => !customerRepo.Any(c => c.AccountNumber == acc && c.Surveys.Count() > 0 && c.Surveys.FirstOrDefault().SurveyRemark==Constants.SURVEY_REMARK_METER_READY));
                    if (notMeterReady.Count() > 0)
                    {
                        throw new AppException($"One or more accounts are not meter ready: {string.Join(", ", notMeterReady)}");
                    }


                    var filePah = SaveFile(file);

                    var installations = new List<Installation>();
                    // update batch items
                    batchItems.ForEach((b) =>
                    {   b.CreatedBy = currentUser.Id;
                        b.CreatedDate = DateTimeOffset.Now;

                        var customer = customerRepo.GetWhere(c => c.AccountNumber == b.AccountNumber).FirstOrDefault();
                        var installation = new Installation()
                        {
                            CustomerId = customer.Id,
                            InstallationStatusId = (long)InstallationStatuses.Pending,
                            SurveyId = customer.Surveys.First().Id,
                            CreatedDate = DateTimeOffset.Now,
                            CreatedBy = currentUser.Id
                        };
                        installations.Add(installation);
                    });

                    // create batch
                    var batch = new InstallationBatch()
                    {
                        FileName = file.FileName,
                        FilePath = filePah,
                        CreatedBy = currentUser.Id,
                        CreatedDate = DateTimeOffset.Now,
                        DateShared = dateShared ?? DateTimeOffset.Now,
                        InstallationBatchItems = batchItems
                    };

                    await batchRepo.Insert(batch);
                    await installationRepo.InsertBulk(installations);

                    //log action
                    await logger.LogActivity(ActivityActionType.CREATE_INSTALLATION_BATCH, currentUser.Email,
                        $"Created batch with file name {file.FileName}, containing {batchItems.Count} installation ready customers");
                }
            }
        }

        public async Task<InstallationBatch> GetBatch(long id)
        {
            return await batchRepo.GetById(id);
        }

        public IEnumerable<InstallationBatch> GetBatches()
        {
            return batchRepo.GetAll().OrderByDescending(b => b.Id);
        }

        // delete batch - only when customers haven't been processed
        public async Task DeleteBatch(long id)
        {
            var batch = await batchRepo.GetById(id);
            if (batch == null)
            {
                throw new AppException($"Invalid batch id");
            }

            // check if any customer is processed
            if (batch.InstallationBatchItems.Any(b => b.Customer.Installations.First().InstallationStatusId != (long)InstallationStatuses.Pending))
            {
                throw new AppException($"Batch cannot be deleted as one or more customer installations are been processed");
            }

            var currentUser = accessor.HttpContext.GetUserSession();

            // delete batch 
            var path = batch.Clone<InstallationBatch>().FilePath;
            var fileName = batch.Clone<InstallationBatch>().FileName;
            var installationIds = batch.InstallationBatchItems.Select(b => b.Customer.Installations.First().Id);
            await installationRepo.DeleteRange(installationIds, false);
            await batchRepo.Delete(id, false);

            DeleteFile(batch.FilePath);

            // log action
            await logger.LogActivity(ActivityActionType.DELETE_INSTALLATION_BATCH, currentUser.Email, batchRepo.TableName, batch, new InstallationBatch(),
                        $"Deleted installation batch with file name {fileName}");

            DeleteFile(path);
        }
    }
}
