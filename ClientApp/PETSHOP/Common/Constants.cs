using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Common
{
    public static class Constants
    {

        //public const string BASE_URI = "https://petshopserver.azurewebsites.net/api/";
        public const string BASE_URI = "https://localhost:44380/api/";
        public const string CLIENT_URI = "https://localhost:44337/";

        // session login 
        public const string VM = "vm";
        public const string VM_MANAGE = "vm_manage";

        //client
        public const string MY_BILL = "mybills";
        //end client


        // server
        public const string BILL = "bills";
        public const string DELIVERY_PRODUCT = "deliveryproducts";
        public const string BILLDETAIL = "billdetails";

        public const string FOOD_PRODUCT = "foodproducts";
        public const string TOY_PRODUCT = "toyproducts";
        public const string COSTUME_PRODUCT = "costumeproducts";

        public const string FOOD = "food";
        public const string TOY = "toys";
        public const string COSTUME = "costume";

        public const string ACCOUNT = "accounts";
        public const string PRODUCT = "products";
        public const string FEEDBACK = "feedbacks";
        public const string USER_COMMENT = "usercomments";
        // end server

        public const string S = "S";
        public const string M = "M";
        public const string L = "L";


        // MESSAGE
        public const string DISCOUNT_INVALID = "Giá trị giảm giá từ 0 đến 1";
        public const string EXTENSION_IMG_NOT_SUPPORT = "Kiểu file không hỗ trợ (jpg, png, ...)";
        public const string IMG_REQUIRED = "Yêu cầu hình cho sản phẩm";

        // Imagemail
        public const string EMBEDED_MAIL_URL = "D:\\OrderImage\\";

        //Role
        public const string ADMIN = "Admin";
        public const string USER = "User";
        public const string CUSTOMER = "Customer";
    }
}
