using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web
{
    public enum AppRoles
    {
        ADMINISTRATOR = 1,
        SUPERVISOR,
        DISCO_PERSONNEL,
        INSTALLER,
        SURVEY_STAFF
    }

    public enum ActivityActionType
    {
        OTHER,
        LOGIN,
        LOGIN_ATTEMPT,

        CREATE_USER,
        UPDATE_USER,
        UPDATE_PASSWORD,
        DELETE_USER,
        UPDATE_APPLICANT,
        UPDATE_APPLICANT_ADDRESS,

        CREATE_METER,
        UPDATE_METER,
        DELETE_METER,

        CREATE_AREA,
        DELETE_AREA,

        CREATE_HOUSE_TYPE,
        UPDATE_HOUSE_TYPE,
        DELETE_HOUSE_TYPE,

        CREATE_APPLIANCE,
        DELETE_APPLIANCE,

        CREATE_PET,
        DELETE_PET,

        CREATE_BATCH,
        DELETE_BATCH,

        CREATE_CUSTOMER,
        UPDATE_CUSTOMER,
        DELETE_CUSTOMER,

        ASSIGN_SURVEY_TASK,
        ASSIGN_SURVEY_TASKS,
        REASSIGN_SURVEY_TASK,
        REASSIGN_SURVEY_TASKS,
        UNASSIGN_SURVEY_TASK,
        UNASSIGN_SURVEY_TASKS,
        SCHEDULE_SURVEY,
        SCHEDULE_SURVEYS,
        COMPLETE_SURVEY,
        UPDATE_SURVEY

    }

    public enum ApplicationsStatus
    {
        Pending=1,
        Submitted,
        Scheduled_for_Installation,
        Installation_In_Progress,
        Installation_Failed,
        Installation_Completed,
        Disco_Confirmation_Failed,
        Disco_Confirmation_Successful
    }

    public class Constants
    {
        public const string SESSION_COOKIE_ID = ".PMAPortal.Session";
        public const string AUTH_COOKIE_ID = ".PMAPortal.Auth";
        public const string CONTEXT_USER_KEY = "Identity";

        public const string ROLE_ADMIN = "Administrator";
        public const string ROLE_SUPERVISOR = "Supervisor";
        public const string ROLE_DISCO = "Disco Personnel";
        public const string ROLE_INSTALLER = "Installer";
        public const string ROLE_SURVEY_STAFF = "Survey Staff";

        public const string USER_TYPE_USER = "User";
        public const string USER_TYPE_APPLICANT = "Applicant";

        public const string CLIENT_TIMEOFFSET_COOKIE_ID = "clientTimeZoneOffset";
        public static string[] IGNORED_COLUMNS = new string[] { "Id", "CreatedBy", "CreatedDate" };

        public const string SURVEY_REMARK_METER_READY = "METER READY";
        public const string SURVEY_REMARK_NOT_METER_READY = "NOT METER READY";
    }
}
