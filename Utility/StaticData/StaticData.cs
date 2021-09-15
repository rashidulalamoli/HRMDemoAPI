using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.StaticData
{
    public static class StaticData
    {
        public const string RESPONSE_TYPE_JSON = "application/json";
        public const string GRANT_TYPE_PASSWORD = "password";
        public const string GRANT_TYPE_REFRESH_TOKEN = "refresh_token";
        public const string JWT_KEY = "Jwt_Key";
        public const string SERVICE_BASE = "service_base";
        public const string ORIGINS = "origins";
        public const string CORS_POLICY = "CorsPolicy";
        public const string MIGRATION_ASSEMBLY = "HRMApi";


        //Routes
        public const string API_CONTROLLER_ROUTE = "api/[controller]";
        public const string TOKEN = "~/token";
        public const string ID = "{id}";



        //Messages
        public const string GRANT_TYPE_NOT_SUPPORTED = "The specified grant type is not supported.";
        public const string SUCCESS = "Success";
        public const string USER_ADD_SUCCESS = "User created successfully";
        public const string USER_UPDATE_SUCCESS = "User updated successfully";
        public const string ERROR_TYPE_NONE = "Something went wrong";
        public const string PASSWORD_MISMATCHED = "Password mismatched";
        public const string PASSWORD_MATCHED = "Password matched";
        public const string USER_NOT_FOUND = "User not found";
        public const string USER_EXIST = "User already exist";
        public const string LEAVE_DELETE_SUCCESS = "Leave deleted successfully";
        public const string LEAVE_UPDATE_SUCCESS = "Leave updated successfully";
        public const string TRAN_IN_PROGRESS = "A transaction is already in progress.";
        public const string UNEXPECTED_HASH_FORMAT = "Unexpected hash format. Should be formatted as `{iterations}.{salt}.{hash}`";
    }
}
