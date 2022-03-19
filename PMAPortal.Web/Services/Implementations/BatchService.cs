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
    public class BatchService: IBatchService
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IRepository<Batch> batchRepo;
        private readonly IRepository<Customer> customerRepo;
        private readonly IOptionsSnapshot<AppSettings> appSettingsDelegate;
        private readonly IHttpContextAccessor accessor;
        private readonly ILoggerService<BatchService> logger;
        private string[] headers = new string[] { "SN", "Date Shared", "Batch", "Account Number", "ARN", "Customer Name", "CIS Name", "Email", "Phone Number", "Address", "CIS Address", "Landmark", "BU","UT", "Feeder", "DT", "Tariff", "Metered Status" };

        public BatchService(
            IWebHostEnvironment hostEnvironment,
            IRepository<Batch> batchRepo,
            IRepository<Customer> customerRepo,
            IOptionsSnapshot<AppSettings> appSettingsDelegate,
            IHttpContextAccessor accessor,
            ILoggerService<BatchService> logger)
        {
            this.hostEnvironment = hostEnvironment;
            this.batchRepo = batchRepo;
            this.customerRepo = customerRepo;
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

        //private bool IsValidDecimal(string value)
        //{
        //    return decimal.TryParse(value, out decimal val);
        //}

        private bool ValidateDataRow(int index, DataRow row, out string errorMessage)
        {
            var err = "";
            var isValid = true;
            if (row[0] == null || Convert.ToString(row[0]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[0]} at row {index}. Field is required.";
            }else if (row[1] == null || !DateTimeOffset.TryParse(Convert.ToString(row[1]).Trim(), out _))
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
            else if (row[5] == null || Convert.ToString(row[5]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[5]} at row {index}. Field is required.";
            }
            else if (row[6] == null || Convert.ToString(row[6]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[6]} at row {index}. Field is required.";
            }
            else if (row[7] == null || Convert.ToString(row[7]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[7]} at row {index}. Field is required.";
            }
            else if (row[8] == null || Convert.ToString(row[8]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[8]} at row {index}. Field is required.";
            }
            else if (row[9] == null || Convert.ToString(row[9]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[9]} at row {index}. Field is required.";
            }
            else if (row[10] == null || Convert.ToString(row[10]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[10]} at row {index}. Field is required.";
            }
            else if (row[11] == null || Convert.ToString(row[11]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[11]} at row {index}. Value is required.";
            }
            else if (row[12] == null || Convert.ToString(row[12]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[12]} at row {index}. Value is required.";
            }
            else if (row[13] == null || Convert.ToString(row[13]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[13]} at row {index}. Value is required.";
            }
            else if (row[14] == null || Convert.ToString(row[14]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[14]} at row {index}. Value is required.";
            }
            else if (row[15] == null || Convert.ToString(row[15]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[15]} at row {index}. Value is required.";
            }
            else if (row[16] == null || Convert.ToString(row[16]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[16]} at row {index}. Value is required.";
            }
            else if (row[17] == null || Convert.ToString(row[17]).Trim().Length == 0)
            {
                isValid = false;
                err = $"Invalid value for {headers[17]} at row {index}. Value is required.";
            }

            errorMessage = err;
            return isValid;
        }

        private IEnumerable<Customer> ExtractData(IFormFile file)
        {
            List<Customer> customers = new List<Customer>();
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
                if(rows.Count <= 1)
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
                        var customer = new Customer()
                        {
                            SN = Convert.ToString(rows[i][0]),
                            DateShared = DateTimeOffset.Parse(Convert.ToString(rows[i][1])),
                            BatchNumber = Convert.ToString(rows[i][2]),
                            AccountNumber = Convert.ToString(rows[i][3]),
                            ARN = Convert.ToString(rows[i][4]),
                            CustomerName = Convert.ToString(rows[i][5]),
                            CISName = Convert.ToString(rows[i][6]),
                            Email = Convert.ToString(rows[i][7]),
                            PhoneNumber = rows[i][8] == null ? null : Convert.ToString(rows[i][8]),
                            Address = Convert.ToString(rows[i][9]),
                            CISAddress = Convert.ToString(rows[i][10]),
                            Landmark = Convert.ToString(rows[i][11]),
                            BU = Convert.ToString(rows[i][12]),
                            UT = Convert.ToString(rows[i][13]),
                            Feeder = Convert.ToString(rows[i][14]),
                            DT = Convert.ToString(rows[i][15]),
                            Tariff = Convert.ToString(rows[i][16]),
                            MeteredStatus = Convert.ToString(rows[i][17]),
                        };

                        customers.Add(customer);
                    }
                }
            }
            fileStream.Dispose();
            return customers;
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

        public async Task AddCustomerBatch(IFormFile file, DateTimeOffset? dateShared = null)
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
                    var customers = ExtractData(file).ToList();

                    // check for duplicate account number
                    var duplicateAccsOnExcel = customers.GroupBy(c => c.AccountNumber).Where(g => g.Count() > 1).Select(g => g.Key);
                    if(duplicateAccsOnExcel.Count() > 0)
                    {
                        throw new AppException($"Duplicate account numbers detected: {string.Join(", ", duplicateAccsOnExcel)}");
                    }

                    var duplicateEmailsOnExcel = customers.GroupBy(c => c.Email).Where(g => g.Count() > 1).Select(g => g.Key);
                    if (duplicateEmailsOnExcel.Count() > 0)
                    {
                        throw new AppException($"Duplicate emails detected: {string.Join(", ", duplicateEmailsOnExcel)}");
                    }

                    var duplicatePhonesOnExcel = customers.GroupBy(c => c.PhoneNumber).Where(g => g.Count() > 1).Select(g => g.Key);
                    if (duplicatePhonesOnExcel.Count() > 0)
                    {
                        throw new AppException($"Duplicate phone numbers detected: {string.Join(", ", duplicatePhonesOnExcel)}");
                    }


                    var accs = customers.Select(c => c.AccountNumber);
                    var duplicateAccsInDb = customerRepo.GetWhere(c => accs.Contains(c.AccountNumber)).Select(c => c.AccountNumber);
                    if(duplicateAccsInDb.Count() > 0)
                    {
                        throw new AppException($"One or more accounts already exist: {string.Join(", ", duplicateAccsInDb)}");
                    }

                    var emails = customers.Select(c => c.Email.ToLower());
                    var duplicateEmailsInDb = customerRepo.GetWhere(c => accs.Contains(c.Email.ToLower())).Select(c => c.Email);
                    if (duplicateEmailsInDb.Count() > 0)
                    {
                        throw new AppException($"One or more emails already exist: {string.Join(", ", duplicateEmailsInDb)}");
                    }

                    var phones = customers.Select(c => c.PhoneNumber);
                    var duplicatePhonesInDb = customerRepo.GetWhere(c => accs.Contains(c.PhoneNumber)).Select(c => c.PhoneNumber);
                    if (duplicatePhonesInDb.Count() > 0)
                    {
                        throw new AppException($"One or more phone numbers already exist: {string.Join(", ", duplicatePhonesInDb)}");
                    }


                    var filePah = SaveFile(file);
                    // update customers
                    customers.ForEach((c) =>
                    {
                        c.CreatedBy = currentUser.Id;
                        c.CreatedDate = DateTimeOffset.Now;
                    });

                    // create batch
                    var batch = new Batch()
                    {
                        FileName = file.FileName,
                        FilePath = filePah,
                        CreatedBy = currentUser.Id,
                        CreatedDate = DateTimeOffset.Now,
                        DateShared = dateShared ?? DateTimeOffset.Now,
                        Customers = customers
                    };

                    await batchRepo.Insert(batch, false);

                    //log action
                    await logger.LogActivity(ActivityActionType.CREATE_BATCH, currentUser.Email,
                        $"Created batch with file name {file.FileName}, containing {customers.Count} customers");
                }
            }
        }

        public async Task<Batch> GetBatch(long id)
        {
            return await batchRepo.GetById(id);
        }

        public IEnumerable<Batch> GetBatches()
        {
            return batchRepo.GetAll().OrderByDescending(b=>b.Id);
        }

        // delete batch - only when customers haven't been processed
        public async Task DeleteBatch(long id)
        {
            var batch = await batchRepo.GetById(id);
            if(batch == null)
            {
                throw new AppException($"Invalid batch id");
            }

            // check if any customer is processed
            if(batch.Customers.Any(c=> c.Surveys.Count() > 0 || c.Installations.Count() > 0))
            {
                throw new AppException($"Batch cannot be deleted as one or more customers are been processed");
            }

            var currentUser = accessor.HttpContext.GetUserSession();

            // delete batch and customers
            var path = batch.Clone<Batch>().FilePath;
            var fileName = batch.Clone<Batch>().FileName;
            await batchRepo.Delete(id, false);
            DeleteFile(batch.FilePath);

            // log action
            await logger.LogActivity(ActivityActionType.DELETE_BATCH, currentUser.Email,
                        $"Deleted batch with file name {fileName}");

            DeleteFile(path);
        }

        //public byte[] ExportBatchToExcel(int id)
        //{
        //    var transactions = db.InwardTransactions.Where(t => t.BatchId == id);

        //    // create excel
        //    var workbook = new XLWorkbook(ClosedXML.Excel.XLEventTracking.Disabled);

        //    // using data table
        //    var table = new DataTable("Inward Transactions");
        //    foreach (var h in InwardHeaders)
        //    {
        //        table.Columns.Add(h, h.ToLower().Contains("amount") || h.ToLower().Contains("rate") ? typeof(decimal) : typeof(string));
        //    }

        //    foreach (var t in transactions)
        //    {
        //        var row = table.NewRow();

        //        row[0] = t.DebitAccount;
        //        row[1] = t.DebitBranch;
        //        row[2] = t.DebitAmount;
        //        row[3] = t.DebitCurrency;
        //        row[4] = t.SessionId;
        //        row[5] = t.CreditAccount;
        //        row[6] = t.CreditBranch;
        //        row[7] = t.CreditAmount;
        //        row[8] = t.CreditCurrency;
        //        row[9] = t.Rate;
        //        row[10] = t.AffiliateCode;
        //        row[11] = t.Narration;

        //        table.Rows.Add(row);
        //    }
        //    workbook.AddWorksheet(table);

        //    byte[] byteFile = null;
        //    using (var stream = new MemoryStream())
        //    {
        //        workbook.SaveAs(stream);
        //        byteFile = stream.ToArray();
        //    }

        //    return byteFile;
        //}


    }
}
