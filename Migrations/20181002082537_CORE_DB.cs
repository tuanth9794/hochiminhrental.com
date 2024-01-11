using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreCnice.Migrations
{
    public partial class CORE_DB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
        name: "CustomerConfirm",
        columns: table => new
        {
            CustomerConfirm_Id = table.Column<int>(nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            CUSTOMER_ID = table.Column<int>(nullable: true),
            CustomerConfirm_Date = table.Column<DateTime>(nullable: true),
            CustomerConfirm_Active = table.Column<int>(nullable: true),
            CustomerConfirm_Type = table.Column<int>(nullable: true),
            CustomerConfirm_Comment = table.Column<string>(nullable: true),
            NEWS_ID = table.Column<int>(nullable: true),
            Customer_request_id = table.Column<int>(nullable: true),
            CustomerConfirm_PublishDate = table.Column<DateTime>(nullable: true)          
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_CustomerConfirm", x => x.CustomerConfirm_Id);
        });

            migrationBuilder.CreateTable(
            name: "Customer_Request",
            columns: table => new
            {
                Customer_request_id = table.Column<decimal>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CUSTOMER_ID = table.Column<int>(nullable: true),
                Customer_Name = table.Column<string>(nullable: true),
                Customer_Email = table.Column<string>(nullable: true),
                Customer_Phone = table.Column<string>(nullable: true),
                Customer_Quoctich = table.Column<string>(nullable: true),
                Customer_DuAn = table.Column<string>(nullable: true),
                Customer_PN = table.Column<int>(nullable: true),
                Customer_PB = table.Column<int>(nullable: true),
                Customer_Dt = table.Column<string>(nullable: true),
                Customer_Ns = table.Column<string>(nullable: true),
                Customer_TimeThue = table.Column<int>(nullable: true),
                Customer_Desc = table.Column<string>(nullable: true),
                Customer_Check = table.Column<int>(nullable: true),
                Customer_Image = table.Column<string>(nullable: true),
                Customer_Active = table.Column<int>(nullable: true),
                Customer_Request_Order = table.Column<int>(nullable: true),
                Customer_Field1 = table.Column<string>(nullable: true),
                Customer_PublishDate = table.Column<DateTime>(nullable: true),
                Customer_TimeAv = table.Column<DateTime>(nullable: true),
                Customer_Field2 = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Customer_Request", x => x.Customer_request_id);
            });

            migrationBuilder.CreateTable(
               name: "PRO_DESC",
               columns: table => new
               {
                   PRO_ID = table.Column<int>(nullable: false)
                       .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                   PRO_NAME = table.Column<string>(nullable: true),
                   PRO_FILE = table.Column<string>(nullable: true),
                   NEWS_ID = table.Column<int>(nullable: true),
                   PRO_ORDER = table.Column<int>(nullable: true),
                   PRO_ACTIVE = table.Column<int>(nullable: true),
                   PRO_TYPE = table.Column<int>(nullable: true),
                   PROPRO_IMAGES = table.Column<string>(nullable: true),
                   PRO_NAME_EN = table.Column<string>(nullable: true),
                   PRO_FILE_EN = table.Column<string>(nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_PRO_DESC", x => x.PRO_ID);
               });


            migrationBuilder.CreateTable(
       name: "ESHOP_CAT_PRO",
       columns: table => new
       {
           ESHOP_CAT_PRO_ID = table.Column<decimal>(nullable: false)
                 .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
           CAT_ID = table.Column<string>(nullable: true),
           PROP_ID = table.Column<string>(nullable: true),
           GROUP_TYPE = table.Column<int>(nullable: true),
           CAT_PRO_FIELD1 = table.Column<string>(nullable: true),
           CAT_PRO_FIELD2 = table.Column<string>(nullable: true)
       },
       constraints: table =>
       {
           table.PrimaryKey("PK_ESHOP_CAT_PRO", x => x.ESHOP_CAT_PRO_ID);
       });

            migrationBuilder.CreateTable(
                name: "ESHOP_AD_ITEM",
                columns: table => new
                {
                    AD_ITEM_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AD_ITEM_CODE = table.Column<string>(nullable: true),
                    AD_ITEM_FILENAME = table.Column<string>(nullable: true),
                    AD_ITEM_DESC = table.Column<string>(nullable: true),
                    AD_ITEM_TYPE = table.Column<int>(nullable: true),
                    AD_ITEM_HEIGHT = table.Column<int>(nullable: true),
                    AD_ITEM_WIDTH = table.Column<int>(nullable: true),
                    AD_ITEM_CLICKCOUNT = table.Column<int>(nullable: true),
                    AD_ITEM_URL = table.Column<string>(nullable: true),
                    AD_ITEM_PUBLISHDATE = table.Column<DateTime>(nullable: true),
                    AD_ITEM_DATEFROM = table.Column<DateTime>(nullable: true),
                    AD_ITEM_DATETO = table.Column<DateTime>(nullable: true),
                    AD_ITEM_ORDER = table.Column<int>(nullable: true),
                    AD_ITEM_POSITION = table.Column<int>(nullable: true),
                    AD_ITEM_TARGET = table.Column<string>(nullable: true),
                    AD_ITEM_COUNT = table.Column<int>(nullable: true),
                    AD_ITEM_LANGUAGE = table.Column<int>(nullable: true),
                    AD_ITEM_FIELD1 = table.Column<string>(nullable: true),
                    AD_ITEM_FIELD2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_AD_ITEM", x => x.AD_ITEM_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_AD_ITEM_CAT",
                columns: table => new
                {
                    AD_ITEM_CAT_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AD_ITEM_ID = table.Column<int>(nullable: true),
                    CAT_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_AD_ITEM_CAT", x => x.AD_ITEM_CAT_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_BASKET",
                columns: table => new
                {
                    BASKET_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CUSTOMER_ID = table.Column<int>(nullable: false),
                    NEWS_ID = table.Column<int>(nullable: false),
                    BASKET_PRICE = table.Column<decimal>(nullable: false),
                    BASKET_QUANTITY = table.Column<int>(nullable: false),
                    BASKET_PUBLISHDATE = table.Column<DateTime>(nullable: false),
                    BASKET_UPDATE = table.Column<DateTime>(nullable: false),
                    BASKET_AMOUNT = table.Column<decimal>(nullable: false),
                    CUSTOMER_OID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_BASKET", x => x.BASKET_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_CATEGORIES",
                columns: table => new
                {
                    CAT_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CAT_NAME = table.Column<string>(nullable: true),
                    CAT_CODE = table.Column<string>(nullable: true),
                    CAT_DESC = table.Column<string>(nullable: true),
                    CAT_URL = table.Column<string>(nullable: true),
                    CAT_TARGET = table.Column<string>(nullable: true),
                    CAT_STATUS = table.Column<int>(nullable: true),
                    CAT_PARENT_ID = table.Column<int>(nullable: true),
                    CAT_PAGEITEM = table.Column<int>(nullable: true),
                    CAT_PERIOD = table.Column<int>(nullable: true),
                    CAT_PERIOD_ORDER = table.Column<int>(nullable: true),
                    CAT_SEO_TITLE_EN = table.Column<string>(nullable: true),
                    CAT_SEO_TITLE = table.Column<string>(nullable: true),
                    CAT_SEO_DESC = table.Column<string>(nullable: true),
                    CAT_SEO_KEYWORD = table.Column<string>(nullable: true),
                    CAT_SEO_URL = table.Column<string>(nullable: true),
                    CAT_SHOWHOME = table.Column<int>(nullable: true),
                    CAT_TYPE = table.Column<int>(nullable: true),
                    CAT_SHOWFOOTER = table.Column<int>(nullable: true),
                    CAT_POSITION = table.Column<int>(nullable: true),
                    CAT_ROWITEM = table.Column<int>(nullable: true),
                    CAT_IMAGE1 = table.Column<string>(nullable: true),
                    CAT_IMAGE2 = table.Column<string>(nullable: true),
                    CAT_IMAGE3 = table.Column<string>(nullable: true),
                    CAT_DESC_EN = table.Column<string>(nullable: true),
                    CAT_FIELD1 = table.Column<string>(nullable: true),
                    CAT_FIELD2 = table.Column<string>(nullable: true),
                    CAT_SEO_URL_EN = table.Column<string>(nullable: true),
                    CAT_NAME_EN = table.Column<string>(nullable: true),
                    CAT_SEO_META_DESC_EN = table.Column<string>(nullable: true),
                    CAT_SEO_META_CANONICAL = table.Column<string>(nullable: true),
                    CAT_LIENKET_EN = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_CATEGORIES", x => x.CAT_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_COLOR",
                columns: table => new
                {
                    COLOR_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COLOR_NAME = table.Column<string>(nullable: true),
                    COLOR_DESC = table.Column<string>(nullable: true),
                    COLOR_PRIORITY = table.Column<int>(nullable: false),
                    COLOR_ACTIVE = table.Column<int>(nullable: false),
                    COLOR_FIELD1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_COLOR", x => x.COLOR_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_COMPA",
                columns: table => new
                {
                    NEWS_COMPA_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NEWS_ID = table.Column<int>(nullable: false),
                    CUSTOMER_ID = table.Column<int>(nullable: false),
                    STATUS = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_COMPA", x => x.NEWS_COMPA_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_CONFIG",
                columns: table => new
                {
                    CONFIG_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CONFIG_TITLE = table.Column<string>(nullable: true),
                    CONFIG_KEYWORD = table.Column<string>(nullable: true),
                    CONFIG_DESCRIPTION = table.Column<string>(nullable: true),
                    CONFIG_HITCOUNTER = table.Column<int>(nullable: true),
                    CONFIG_FAVICON = table.Column<string>(nullable: true),
                    CONFIG_ORDER = table.Column<int>(nullable: true),
                    CONFIG_LANGUAGE = table.Column<int>(nullable: true),
                    CONFIG_EMAIL = table.Column<string>(nullable: true),
                    CONFIG_SMTP = table.Column<string>(nullable: true),
                    CONFIG_PORT = table.Column<string>(nullable: true),
                    CONFIG_PASSWORD = table.Column<string>(nullable: true),
                    CONFIG_NAME = table.Column<string>(nullable: true),
                    CONFIG_ADD = table.Column<string>(nullable: true),
                    CONFIG_FACEBOOK = table.Column<string>(nullable: true),
                    CONFIG_GOOGLE = table.Column<string>(nullable: true),
                    CONFIG_NAME_US = table.Column<string>(nullable: true),
                    CONFIG_FOOTER = table.Column<string>(nullable: true),
                    CONFIG_CONTACT = table.Column<string>(nullable: true),
                    CONFIG_DESCRIPTION_EN = table.Column<string>(nullable: true),
                    CONFIG_FIELD1 = table.Column<string>(nullable: true),
                    CONFIG_FIELD2 = table.Column<string>(nullable: true),
                    CONFIG_FIELD3 = table.Column<string>(nullable: true),
                    CONFIG_FIELD4 = table.Column<string>(nullable: true),
                    CONFIG_FIELD5 = table.Column<string>(nullable: true)

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_CONFIG", x => x.CONFIG_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_CONTACT",
                columns: table => new
                {
                    CONTACT_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CONTACT_NAME = table.Column<string>(nullable: true),
                    CONTACT_EMAIL = table.Column<string>(nullable: true),
                    CONTACT_PHONE = table.Column<string>(nullable: true),
                    CONTACT_ADDRESS = table.Column<string>(nullable: true),
                    CONTACT_TITLE = table.Column<string>(nullable: true),
                    CONTACT_CONTENT = table.Column<string>(nullable: true),
                    CONTACT_ATT1 = table.Column<string>(nullable: true),
                    CONTACT_PUBLISHDATE = table.Column<DateTime>(nullable: true),
                    CONTACT_ANSWER = table.Column<string>(nullable: true),
                    CONTACT_SHOWTYPE = table.Column<int>(nullable: true),
                    CONTACT_TYPE = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_CONTACT", x => x.CONTACT_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_CUSTOMER",
                columns: table => new
                {
                    CUSTOMER_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CUSTOMER_FULLNAME = table.Column<string>(nullable: true),
                    CUSTOMER_UN = table.Column<string>(nullable: true),
                    CUSTOMER_PW = table.Column<string>(nullable: true),
                    CUSTOMER_SEX = table.Column<int>(nullable: true),
                    CUSTOMER_ADDRESS = table.Column<string>(nullable: true),
                    CUSTOMER_EMAIL = table.Column<string>(nullable: true),
                    CUSTOMER_FIELD3 = table.Column<string>(nullable: true),
                    CUSTOMER_FIELD4 = table.Column<string>(nullable: true),
                    CUSTOMER_PHONE1 = table.Column<string>(nullable: true),
                    CUSTOMER_PHONE2 = table.Column<string>(nullable: true),
                    CUSTOMER_NEWSLETTER = table.Column<int>(nullable: true),
                    CUSTOMER_PUBLISHDATE = table.Column<DateTime>(nullable: true),
                    CUSTOMER_UPDATE = table.Column<DateTime>(nullable: true),
                    CUSTOMER_OID = table.Column<Guid>(nullable: true),
                    CUSTOMER_SHOWTYPE = table.Column<int>(nullable: true),
                    CUSTOMER_TOTAL_POINT = table.Column<int>(nullable: true),
                    CUSTOMER_REMAIN = table.Column<int>(nullable: true),
                    CUSTOMER_FIELD1 = table.Column<string>(nullable: true),
                    CUSTOMER_IP = table.Column<string>(nullable: true),

                    CUSTOMER_NGANSACH = table.Column<string>(nullable: true),
                    CUSTOMER_KHUVU = table.Column<string>(nullable: true),
                    CUSTOMER_LOAICANHO = table.Column<string>(nullable: true),
                    CUSTOMER_THOIGIANTHUE = table.Column<string>(nullable: true),
                    CUSTOMER_SONGUOIO = table.Column<string>(nullable: true),
                    CUSTOMER_ACTIVE = table.Column<int>(nullable: true),
                    USER_ID = table.Column<int>(nullable: true),
                    USER_ID_PHUTRACH = table.Column<int>(nullable: true),
                    CUSTOMER_HIDEN = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_CUSTOMER", x => x.CUSTOMER_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_EMAIL",
                columns: table => new
                {
                    EMAIL_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EMAIL_STT = table.Column<int>(nullable: false),
                    EMAIL_DESC = table.Column<string>(nullable: true),
                    EMAIL_FROM = table.Column<string>(nullable: true),
                    EMAIL_TO = table.Column<string>(nullable: true),
                    EMAIL_CC = table.Column<string>(nullable: true),
                    EMAIL_BCC = table.Column<string>(nullable: true),
                    EMAIL_PUBLISHDATE_DAY = table.Column<int>(nullable: true),
                    EMAIL_CUSTOMER_ALL = table.Column<int>(nullable: true),
                    EMAIL_PUBLISHDATE = table.Column<DateTime>(nullable: true),
                    EMAIL_PUBLISHDATE_START = table.Column<DateTime>(nullable: true),
                    USER_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_EMAIL", x => x.EMAIL_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_GIFT",
                columns: table => new
                {
                    GIFT_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GIFT_NAME = table.Column<string>(nullable: true),
                    GIFT_DESC = table.Column<string>(nullable: true),
                    GIFT_IMAGE = table.Column<string>(nullable: true),
                    GIFT_POINT = table.Column<int>(nullable: false),
                    GIFT_AMOUNT = table.Column<int>(nullable: false),
                    GIFT_MAX_AMOUNT = table.Column<int>(nullable: false),
                    GIFT_CONTENT = table.Column<string>(nullable: true),
                    GIFT_PULISHDATE = table.Column<DateTime>(nullable: false),
                    GIF_ACTIVE = table.Column<int>(nullable: false),
                    GIFT_USE_PUBLISHDATE = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_GIFT", x => x.GIFT_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_GROUP_CAT",
                columns: table => new
                {
                    GROUP_CAT_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GROUP_ID = table.Column<int>(nullable: false),
                    CAT_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_GROUP_CAT", x => x.GROUP_CAT_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_MONEY",
                columns: table => new
                {
                    NEWS_MONEY_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NEWS_MONEY_TITLE = table.Column<string>(nullable: true),
                    NEWS_MONEY_BEGIN = table.Column<string>(nullable: true),
                    NEWS_MONEY_END = table.Column<string>(nullable: true),
                    NEWS_MONEY_ACTIVE = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_MONEY", x => x.NEWS_MONEY_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_NEWS",
                columns: table => new
                {
                    NEWS_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NEWS_CODE = table.Column<string>(nullable: true),
                    NEWS_TITLE = table.Column<string>(nullable: true),
                    NEWS_DESC = table.Column<string>(nullable: true),
                    NEWS_URL = table.Column<string>(nullable: true),
                    NEWS_TARGET = table.Column<string>(nullable: true),
                    NEWS_SEO_KEYWORD = table.Column<string>(nullable: true),
                    NEWS_SEO_DESC = table.Column<string>(nullable: true),
                    NEWS_SEO_TITLE = table.Column<string>(nullable: true),
                    NEWS_SEO_URL = table.Column<string>(nullable: true),
                    NEWS_FILEHTML = table.Column<string>(nullable: true),
                    NEWS_PUBLISHDATE = table.Column<DateTime>(nullable: true),
                    NEWS_UPDATE = table.Column<DateTime>(nullable: true),
                    NEWS_SHOWTYPE = table.Column<int>(nullable: true),
                    NEWS_SHOWINDETAIL = table.Column<int>(nullable: true),
                    NEWS_FEEDBACKTYPE = table.Column<int>(nullable: true),
                    NEWS_TYPE = table.Column<int>(nullable: true),
                    NEWS_PERIOD = table.Column<int>(nullable: true),
                    NEWS_ORDER_PERIOD = table.Column<int>(nullable: true),
                    NEWS_ORDER = table.Column<int>(nullable: true),
                    NEWS_PRINTTYPE = table.Column<int>(nullable: true),
                    NEWS_COUNT = table.Column<int>(nullable: true),
                    NEWS_SENDEMAIL = table.Column<int>(nullable: true),
                    NEWS_SENDDATE = table.Column<int>(nullable: true),
                    NEWS_PRICE1 = table.Column<decimal>(nullable: true),
                    NEWS_PRICE2 = table.Column<decimal>(nullable: true),
                    NEWS_PRICE3 = table.Column<decimal>(nullable: true),
                    NEWS_IMAGE1 = table.Column<string>(nullable: true),
                    NEWS_IMAGE2 = table.Column<string>(nullable: true),
                    NEWS_IMAGE3 = table.Column<string>(nullable: true),
                    NEWS_IMAGE4 = table.Column<string>(nullable: true),
                    NEWS_FIELD1 = table.Column<string>(nullable: true),
                    NEWS_FIELD2 = table.Column<string>(nullable: true),
                    NEWS_FIELD3 = table.Column<string>(nullable: true),
                    NEWS_FIELD4 = table.Column<string>(nullable: true),
                    NEWS_TITLE_EN = table.Column<string>(nullable: true),
                    NEWS_DESC_EN = table.Column<string>(nullable: true),
                    NEWS_FIELD5 = table.Column<string>(nullable: true),
                    NEWS_FILEHTML_EN = table.Column<string>(nullable: true),
                    NEWS_HTML_EN1 = table.Column<string>(nullable: true),
                    NEWS_HTML_EN2 = table.Column<string>(nullable: true),
                    NEWS_HTML_EN3 = table.Column<string>(nullable: true),
                    NEWS_TITLE_JS = table.Column<string>(nullable: true),
                    NEWS_SEO_DESC_JS = table.Column<string>(nullable: true),
                    NEWS_SEO_URL_JS = table.Column<string>(nullable: true),
                    NEWS_SEO_URL_EN = table.Column<string>(nullable: true),
                    NEWS_SEO_META_DESC_EN = table.Column<string>(nullable: true),
                    NEWS_SEO_META_CANONICAL = table.Column<string>(nullable: true),
                    USER_ID = table.Column<int>(nullable: true),
                    NEWS_LIENKET_EN = table.Column<string>(nullable: true),
                    NEWS_TIME_AVALBLE = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_NEWS", x => x.NEWS_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_NEWS_ATT",
                columns: table => new
                {
                    NEWS_ATT_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NEWS_ATT_NAME = table.Column<string>(nullable: true),
                    NEWS_ATT_FILE = table.Column<string>(nullable: true),
                    NEWS_ATT_URL = table.Column<string>(nullable: true),
                    NEWS_ATT_ORDER = table.Column<int>(nullable: false),
                    EXT_ID = table.Column<int>(nullable: false),
                    NEWS_ID = table.Column<int>(nullable: false),
                    NEWS_ATT_FIELD1 = table.Column<string>(nullable: true),
                    NEWS_ATT_FIELD2 = table.Column<string>(nullable: true),
                    NEWS_ATT_FIELD3 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_NEWS_ATT", x => x.NEWS_ATT_ID);
                });

            migrationBuilder.CreateTable(
               name: "NEWS_CUSTOMER_POST",
               columns: table => new
               {
                   NEWS_CUS_ID = table.Column<int>(nullable: false)
                       .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                   CUSTOMER_ID = table.Column<int>(nullable: false),
                   NEWS_ID = table.Column<int>(nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_NEWS_CUSTOMER_POST", x => x.NEWS_CUS_ID);
               });

            migrationBuilder.CreateTable(
                name: "ESHOP_NEWS_CAT",
                columns: table => new
                {
                    NEWS_CAT_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NEWS_ID = table.Column<int>(nullable: false),
                    CAT_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_NEWS_CAT", x => x.NEWS_CAT_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_NEWS_COMMENT",
                columns: table => new
                {
                    COMMENT_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMMENT_NAME = table.Column<string>(nullable: true),
                    COMMENT_EMAIL = table.Column<string>(nullable: true),
                    COMMENT_CONTENT = table.Column<string>(nullable: true),
                    COMMENT_STATUS = table.Column<int>(nullable: false),
                    CUS_ID = table.Column<int>(nullable: false),
                    NEWS_ID = table.Column<int>(nullable: false),
                    COMMENT_PUBLISHDATE = table.Column<DateTime>(nullable: false),
                    COMMENT_FIELD1 = table.Column<string>(nullable: true),
                    COMMENT_FIELD2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_NEWS_COMMENT", x => x.COMMENT_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_NEWS_IMAGE",
                columns: table => new
                {
                    NEWS_IMG_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NEWS_IMG_IMAGE1 = table.Column<string>(nullable: true),
                    NEWS_IMG_DESC = table.Column<string>(nullable: true),
                    NEWS_IMG_ORDER = table.Column<int>(nullable: false),
                    NEWS_IMG_SHOWTYPE = table.Column<int>(nullable: true),
                    NEWS_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_NEWS_IMAGE", x => x.NEWS_IMG_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_NEWS_NEWS",
                columns: table => new
                {
                    NEWS_NEWS_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NEWS_ID1 = table.Column<int>(nullable: false),
                    NEWS_ID2 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_NEWS_NEWS", x => x.NEWS_NEWS_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_NEWS_PROPERTIES",
                columns: table => new
                {
                    NEWS_PROP_ID = table.Column<decimal>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NEWS_ID = table.Column<decimal>(nullable: false),
                    PROP_ID = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_NEWS_PROPERTIES", x => x.NEWS_PROP_ID);
                });

            migrationBuilder.CreateTable(
            name: "ESHOP_CUSTOMER_PROPERTIES",
            columns: table => new
            {
                CUSTOMER_PROP_ID = table.Column<decimal>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CUSTOMER_ID = table.Column<decimal>(nullable: false),
                PROP_ID = table.Column<decimal>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ESHOP_CUSTOMER_PROPERTIES", x => x.CUSTOMER_PROP_ID);
            });

            migrationBuilder.CreateTable(
                name: "ESHOP_ONLINE",
                columns: table => new
                {
                    ONLINE_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ONLINE_NICKNAME = table.Column<string>(nullable: true),
                    ONLINE_DESC = table.Column<string>(nullable: true),
                    ONLINE_IMAGE = table.Column<string>(nullable: true),
                    ONLINE_ORDER = table.Column<int>(nullable: false),
                    ONLINE_TYPE = table.Column<int>(nullable: false),
                    CUS_ID = table.Column<int>(nullable: true),
                    ONLINE_OUT_DATE = table.Column<DateTime>(nullable: true),
                    ONLINE_START_BEGIN_DATE = table.Column<DateTime>(nullable: true),
                    ONLINE_DESC_EN = table.Column<string>(nullable: true),
                    ONLINE_IP = table.Column<string>(nullable: true),
                    ONLINE_LOCATION = table.Column<string>(nullable: true),
                    ONLINE_COMBACK = table.Column<string>(nullable: true),
                    ONLINE_LINK = table.Column<string>(nullable: true),
                    ONLINE_ACTIVE = table.Column<int>(nullable: true),
                    ONLINE_GUID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_ONLINE", x => x.ONLINE_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_ORDER",
                columns: table => new
                {
                    ORDER_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CUSTOMER_ID = table.Column<int>(nullable: false),
                    ORDER_CODE = table.Column<string>(nullable: true),
                    ORDER_TOTAL_ALL = table.Column<decimal>(nullable: true),
                    ORDER_PUBLISHDATE = table.Column<DateTime>(nullable: true),
                    ORDER_UPDATE = table.Column<DateTime>(nullable: true),
                    RECEIVER_FULLNAME = table.Column<string>(nullable: true),
                    RECEIVER_ADDRESS = table.Column<string>(nullable: true),
                    RECEIVER_PHONE1 = table.Column<string>(nullable: true),
                    RECEIVER_EMAIL = table.Column<string>(nullable: true),
                    ORDER_DESC = table.Column<string>(nullable: true),
                    ORDER_STATUS = table.Column<int>(nullable: true),
                    ORDER_DAY = table.Column<int>(nullable: true),
                    ORDER_MONTH = table.Column<int>(nullable: true),
                    ORDER_YEAR = table.Column<int>(nullable: true),
                    ORDER_IP = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_ORDER", x => x.ORDER_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_ORDER_ITEM",
                columns: table => new
                {
                    ITEM_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ORDER_ID = table.Column<int>(nullable: false),
                    NEWS_ID = table.Column<int>(nullable: false),
                    UNIT_ID = table.Column<int>(nullable: false),
                    ITEM_QUANTITY = table.Column<float>(nullable: false),
                    ITEM_SUBTOTAL = table.Column<decimal>(nullable: false),
                    ITEM_PUBLISDATE = table.Column<DateTime>(nullable: false),
                    ITEM_UPDATE = table.Column<DateTime>(nullable: false),
                    ITEM_PRICE = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_ORDER_ITEM", x => x.ITEM_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_PROPERTIES",
                columns: table => new
                {
                    PROP_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PROP_NAME = table.Column<string>(nullable: true),
                    PRO_NAME_EN = table.Column<string>(nullable: true),                    
                    PROP_DESC = table.Column<string>(nullable: true),
                    PROP_PARENT_ID = table.Column<int>(nullable: false),
                    PROP_ACTIVE = table.Column<int>(nullable: true),
                    PRO_TYPE = table.Column<int>(nullable: true),
                    PROP_RANK = table.Column<int>(nullable: true),
                    PRO_TYPE_HOME = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_PROPERTIES", x => x.PROP_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_STORE",
                columns: table => new
                {
                    ESHOP_STORE_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ESHOP_STORE_NAME = table.Column<string>(nullable: true),
                    ESHOP_STORE_ADDRESS = table.Column<string>(nullable: true),
                    ESHOP_STORE_EMAIL = table.Column<string>(nullable: true),
                    ESHOP_STORE_PHONE = table.Column<string>(nullable: true),
                    ESHOP_STORE_IMAGE = table.Column<string>(nullable: true),
                    ESHOP_STORE_UN = table.Column<string>(nullable: true),
                    ESHOP_STORE_ACTIVE = table.Column<int>(nullable: true),
                    ESHOP_STORE_TYPE = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_STORE", x => x.ESHOP_STORE_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_THEME",
                columns: table => new
                {
                    ESHOP_THEME_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ESHOP_THEME_NAME = table.Column<string>(nullable: true),
                    ESHOP_THEME_LINK_FILE = table.Column<string>(nullable: true),
                    CAT_ID = table.Column<int>(nullable: false),
                    NEWS_ID = table.Column<int>(nullable: false),
                    ESHOP_THEME_TYPE = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_THEME", x => x.ESHOP_THEME_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_UNITS",
                columns: table => new
                {
                    UNIT_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UNIT_NAME = table.Column<string>(nullable: true),
                    UNIT_DESC = table.Column<string>(nullable: true),
                    UNIT_PRICE = table.Column<decimal>(nullable: false),
                    UNIT_ACTIVE = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_UNITS", x => x.UNIT_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_USER",
                columns: table => new
                {
                    USER_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    USER_NAME = table.Column<string>(nullable: true),
                    USER_UN = table.Column<string>(nullable: true),
                    USER_PW = table.Column<string>(nullable: true),
                    GROUP_ID = table.Column<int>(nullable: true),
                    USER_TYPE = table.Column<int>(nullable: true),
                    USER_ACTIVE = table.Column<int>(nullable: true),
                    SALT = table.Column<string>(nullable: true),
                    USER_PUBLISHDATE = table.Column<DateTime>(nullable: true),
                    USER_START_WORK = table.Column<DateTime>(nullable: true),
                    USER_END_WORK = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_USER", x => x.USER_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_USER_GIFT",
                columns: table => new
                {
                    USER_GIFT_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CUSTOMER_ID = table.Column<int>(nullable: false),
                    GIFT_ID = table.Column<int>(nullable: false),
                    USER_GIFT_DATE = table.Column<DateTime>(nullable: false),
                    USER_GIFT_STATUS = table.Column<int>(nullable: false),
                    USER_GIFT_QUANTITY = table.Column<int>(nullable: false),
                    USER_GIFT_DESC = table.Column<string>(nullable: true),
                    USER_GIFT_FEILD = table.Column<string>(nullable: true),
                    USER_GIFT_FIELD1 = table.Column<string>(nullable: true),
                    USER_GIFT_ACTIVE = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_USER_GIFT", x => x.USER_GIFT_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_WEBLINKS",
                columns: table => new
                {
                    WEBSITE_LINKS_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WEBSITE_LINKS_NAME = table.Column<string>(nullable: true),
                    WEBSITE_LINKS_URL = table.Column<string>(nullable: true),
                    WEBSITE_LINKS_TARGET = table.Column<string>(nullable: true),
                    WEBSITE_LINKS_LANGUAGE = table.Column<int>(nullable: false),
                    WEBSITE_LINKS_ORDER = table.Column<int>(nullable: false),
                    WEBSITE_LINK_PE_ID = table.Column<int>(nullable: false),
                    WEBSITE_RANK = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_WEBLINKS", x => x.WEBSITE_LINKS_ID);
                });

            migrationBuilder.CreateTable(
                name: "MODULE",
                columns: table => new
                {
                    NEWS_MODULE_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NEWS_MODULE_NAME = table.Column<string>(nullable: true),
                    NEWS_MODULE_LANG = table.Column<int>(nullable: false),
                    NEWS_MODULE_STATUS = table.Column<int>(nullable: false),
                    NEWS_MODULE_ORDER = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MODULE", x => x.NEWS_MODULE_ID);
                });

            migrationBuilder.CreateTable(
                name: "ESHOP_GROUP",
                columns: table => new
                {
                    GROUP_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GROUP_CODE = table.Column<string>(nullable: true),
                    GROUP_NAME = table.Column<string>(nullable: true),
                    GROUP_TYPE = table.Column<int>(nullable: true),
                    GROUP_PUBLISHDATE = table.Column<DateTime>(nullable: true),
                    ESHOP_USERUSER_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESHOP_GROUP", x => x.GROUP_ID);
                    table.ForeignKey(
                        name: "FK_ESHOP_GROUP_ESHOP_USER_ESHOP_USERUSER_ID",
                        column: x => x.ESHOP_USERUSER_ID,
                        principalTable: "ESHOP_USER",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ESHOP_GROUP_ESHOP_USERUSER_ID",
                table: "ESHOP_GROUP",
                column: "ESHOP_USERUSER_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "Customer_Request");

            migrationBuilder.DropTable(
                name: "ESHOP_AD_ITEM");

            migrationBuilder.DropTable(
                name: "ESHOP_AD_ITEM_CAT");

            migrationBuilder.DropTable(
                name: "ESHOP_BASKET");

            migrationBuilder.DropTable(
                name: "ESHOP_CATEGORIES");

            migrationBuilder.DropTable(
                name: "ESHOP_COLOR");

            migrationBuilder.DropTable(
                name: "ESHOP_COMPA");

            migrationBuilder.DropTable(
                name: "ESHOP_CONFIG");

            migrationBuilder.DropTable(
                name: "ESHOP_CONTACT");

            migrationBuilder.DropTable(
                name: "ESHOP_CUSTOMER");

            migrationBuilder.DropTable(
                name: "ESHOP_EMAIL");

            migrationBuilder.DropTable(
                name: "ESHOP_GIFT");

            migrationBuilder.DropTable(
                name: "ESHOP_GROUP");

            migrationBuilder.DropTable(
                name: "ESHOP_GROUP_CAT");

            migrationBuilder.DropTable(
                name: "ESHOP_MONEY");

            migrationBuilder.DropTable(
                name: "ESHOP_NEWS");

            migrationBuilder.DropTable(
                name: "ESHOP_NEWS_ATT");

            migrationBuilder.DropTable(
                name: "ESHOP_NEWS_CAT");

            migrationBuilder.DropTable(
                name: "ESHOP_NEWS_COMMENT");

            migrationBuilder.DropTable(
                name: "ESHOP_NEWS_IMAGE");

            migrationBuilder.DropTable(
                name: "ESHOP_NEWS_NEWS");

            migrationBuilder.DropTable(
                name: "ESHOP_NEWS_PROPERTIES");

            migrationBuilder.DropTable(
                name: "ESHOP_ONLINE");

            migrationBuilder.DropTable(
                name: "ESHOP_ORDER");

            migrationBuilder.DropTable(
                name: "ESHOP_ORDER_ITEM");

            migrationBuilder.DropTable(
                name: "ESHOP_PROPERTIES");

            migrationBuilder.DropTable(
                name: "ESHOP_STORE");

            migrationBuilder.DropTable(
                name: "ESHOP_THEME");

            migrationBuilder.DropTable(
                name: "ESHOP_UNITS");

            migrationBuilder.DropTable(
                name: "ESHOP_USER_GIFT");

            migrationBuilder.DropTable(
                name: "ESHOP_WEBLINKS");

            migrationBuilder.DropTable(
                name: "MODULE");

            migrationBuilder.DropTable(
                name: "ESHOP_USER");
        }
    }
}
