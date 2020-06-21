using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Common
{
    public static class Constants
    {
        public const string BASE_URI = "https://localhost:44380/api/";

        public const string FOOD_PRODUCT = "foodproducts";
        public const string TOY_PRODUCT = "toyproducts";
        public const string COSTUME_PRODUCT = "costumeproducts";

        public const string FOOD = "food";
        public const string TOY = "toys";
        public const string COSTUME = "costume";

        public const string ACCOUNT = "accounts";
        public const string PRODUCT = "products";
        public const string FEEDBACK = "feedbacks";

        public const string S = "S";
        public const string M = "M";
        public const string L = "L";


        // MESSAGE
        public const string DISCOUNT_INVALID = "Giá trị giảm giá từ 0 đến 1";
        public const string EXTENSION_IMG_NOT_SUPPORT = "Kiểu file không hỗ trợ (jpg, png, ...)";
        public const string IMG_REQUIRED = "Yêu cầu hình cho sản phẩm";
    }
}
