USE [master]
GO
/****** Object:  Database [Repair_service]    Script Date: 23.05.2024 20:33:07 ******/
CREATE DATABASE [Repair_service]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Repair_service', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Repair_service.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Repair_service_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Repair_service_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Repair_service] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Repair_service].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Repair_service] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Repair_service] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Repair_service] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Repair_service] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Repair_service] SET ARITHABORT OFF 
GO
ALTER DATABASE [Repair_service] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Repair_service] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Repair_service] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Repair_service] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Repair_service] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Repair_service] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Repair_service] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Repair_service] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Repair_service] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Repair_service] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Repair_service] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Repair_service] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Repair_service] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Repair_service] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Repair_service] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Repair_service] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Repair_service] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Repair_service] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Repair_service] SET  MULTI_USER 
GO
ALTER DATABASE [Repair_service] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Repair_service] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Repair_service] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Repair_service] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Repair_service] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Repair_service] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Repair_service] SET QUERY_STORE = OFF
GO
USE [Repair_service]
GO
/****** Object:  Table [dbo].[Accessories]    Script Date: 23.05.2024 20:33:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accessories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[Manufacturer] [nvarchar](50) NOT NULL,
	[Cost] [money] NOT NULL,
	[SupplierId] [int] NOT NULL,
 CONSTRAINT [PK_Accessories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 23.05.2024 20:33:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Surname] [nvarchar](50) NOT NULL,
	[Patronymic] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NOT NULL,
	[Phone_number] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[RoleId] [int] NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Devices]    Script Date: 23.05.2024 20:33:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Devices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Model] [nvarchar](50) NOT NULL,
	[Manufacturer] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Year_of_release] [smallint] NOT NULL,
	[Serial_number] [int] NULL,
	[ClientId] [int] NOT NULL,
	[Photo] [nvarchar](50) NULL,
 CONSTRAINT [PK_Devices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderAccessories]    Script Date: 23.05.2024 20:33:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderAccessories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[AccessoriesId] [int] NOT NULL,
	[Count] [nvarchar](50) NOT NULL,
	[Cost] [money] NOT NULL,
	[Date_order] [date] NOT NULL,
	[Status_order] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_OrderAccessories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Repairs]    Script Date: 23.05.2024 20:33:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Repairs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeviceId] [int] NOT NULL,
	[StaffId] [int] NOT NULL,
	[Date_of_admission] [date] NOT NULL,
	[End_date] [date] NULL,
	[Cost] [money] NOT NULL,
	[Description_of_problem] [nvarchar](500) NOT NULL,
	[Descriprion_of_work_done] [nvarchar](500) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Repairs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 23.05.2024 20:33:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 23.05.2024 20:33:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Surname] [nvarchar](50) NOT NULL,
	[Patronymic] [nvarchar](50) NULL,
	[Experiance] [int] NULL,
	[Post] [nvarchar](50) NOT NULL,
	[Salary] [money] NOT NULL,
	[Date_of_employment] [date] NOT NULL,
	[Photo] [nvarchar](50) NULL,
	[RoleId] [int] NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suppliers]    Script Date: 23.05.2024 20:33:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suppliers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Company_name] [nvarchar](50) NOT NULL,
	[Contact_person] [nvarchar](50) NULL,
	[Phone_number] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Suppliers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Accessories] ON 

INSERT [dbo].[Accessories] ([Id], [Name], [Description], [Manufacturer], [Cost], [SupplierId]) VALUES (1, N'Аккумулятор для ноутбуков HP', N'Литиевый аккумулятор, подходящий для большинства моделей ноутбуков HP', N'HP', 1000.0000, 1)
INSERT [dbo].[Accessories] ([Id], [Name], [Description], [Manufacturer], [Cost], [SupplierId]) VALUES (2, N'Аккумулятор для ноутбуков ASUS', N'Литиевый аккумулятор, подходящий для большинства моделей ноутбуков ASUS', N'ASUS', 1500.0000, 1)
INSERT [dbo].[Accessories] ([Id], [Name], [Description], [Manufacturer], [Cost], [SupplierId]) VALUES (3, N'Картиридж для лазерного принтера', NULL, N'Pantum', 2000.0000, 1)
INSERT [dbo].[Accessories] ([Id], [Name], [Description], [Manufacturer], [Cost], [SupplierId]) VALUES (4, N'Внутренние модули для ноутбука', N'Набор, состоящий из схем, плат, различной электронники для ноутбуков.', N'ASUS', 3000.0000, 2)
INSERT [dbo].[Accessories] ([Id], [Name], [Description], [Manufacturer], [Cost], [SupplierId]) VALUES (5, N'Плата для сматрфона HONOR', NULL, N'HONOR', 7000.0000, 2)
SET IDENTITY_INSERT [dbo].[Accessories] OFF
GO
SET IDENTITY_INSERT [dbo].[Clients] ON 

INSERT [dbo].[Clients] ([Id], [Name], [Surname], [Patronymic], [Address], [Phone_number], [Email], [RoleId], [Login], [Password]) VALUES (1, N'Егор', N'Давыдов', N'Тимофеевич', N'г. Рязань ул. Крупской д. 11', N'+7(927)973-61-56', N'Davidov@mail.ru', 1, N'egor', N'DraGon@fl1ght')
INSERT [dbo].[Clients] ([Id], [Name], [Surname], [Patronymic], [Address], [Phone_number], [Email], [RoleId], [Login], [Password]) VALUES (2, N'Варвара', N'Павловна', N'Степановна', N'г. Рязань ул. Новаторов д. 10', N'+7(345)654-23-78', N'PavlovnaV@gmail.com', 1, N'VPS', N'Tr1cK$tar!567')
INSERT [dbo].[Clients] ([Id], [Name], [Surname], [Patronymic], [Address], [Phone_number], [Email], [RoleId], [Login], [Password]) VALUES (3, N'Дмитрий', N'Попов', N'Даниилович', N'г. Рязань ул. Костычева д. 8', N'+7(965)543-78-65', N'DmitriyP@yandex.ru', 1, N'DPopov', N'P@ssw0rd!Lock#321')
INSERT [dbo].[Clients] ([Id], [Name], [Surname], [Patronymic], [Address], [Phone_number], [Email], [RoleId], [Login], [Password]) VALUES (4, N'Александр', N'Петров', N'Григорьевич', N'г. Рязань ул. Мервинская д. 119', N'+7(976)874-32-12', N'AGPetrov@gmail.com', 1, N'AGPetrov', N'B0unc3J0y$ea_99')
INSERT [dbo].[Clients] ([Id], [Name], [Surname], [Patronymic], [Address], [Phone_number], [Email], [RoleId], [Login], [Password]) VALUES (5, N'Лев', N'Смирнов', N'Львович', N'г. Рязань ул. Советская д. 142', N'+7(954)789-00-31', N'Lion@yandex.ru', 1, N'Lion123', N'F1reIc3@Bl@ze$778')
SET IDENTITY_INSERT [dbo].[Clients] OFF
GO
SET IDENTITY_INSERT [dbo].[Devices] ON 

INSERT [dbo].[Devices] ([Id], [Model], [Manufacturer], [Type], [Year_of_release], [Serial_number], [ClientId], [Photo]) VALUES (1, N'Pavilion 15 Gaming', N'HP', N'Ноутбук', 2020, NULL, 1, N'Pavilion15Gaming.jpg')
INSERT [dbo].[Devices] ([Id], [Model], [Manufacturer], [Type], [Year_of_release], [Serial_number], [ClientId], [Photo]) VALUES (2, N'P2516/P2518', N'Pantum', N'Принтер лазерный', 2017, 6574896, 2, N'P2516.jpg')
INSERT [dbo].[Devices] ([Id], [Model], [Manufacturer], [Type], [Year_of_release], [Serial_number], [ClientId], [Photo]) VALUES (3, N'VivoBook R410MA-212.BK128', N'ASUS', N'Ноутбук', 2022, NULL, 3, N'VivoBook.jpg')
INSERT [dbo].[Devices] ([Id], [Model], [Manufacturer], [Type], [Year_of_release], [Serial_number], [ClientId], [Photo]) VALUES (4, N'P2500', N'Pantum', N'Принтер лазерный', 2015, 3254295, 4, NULL)
INSERT [dbo].[Devices] ([Id], [Model], [Manufacturer], [Type], [Year_of_release], [Serial_number], [ClientId], [Photo]) VALUES (5, N'Magic4 Pro', N'HONOR', N'Смартфон', 2023, 8647552, 5, N'Magic4Pro.jpg')
SET IDENTITY_INSERT [dbo].[Devices] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderAccessories] ON 

INSERT [dbo].[OrderAccessories] ([Id], [ClientId], [AccessoriesId], [Count], [Cost], [Date_order], [Status_order]) VALUES (1, 1, 1, N'1 штука', 1000.0000, CAST(N'2023-03-01' AS Date), N'Получен')
INSERT [dbo].[OrderAccessories] ([Id], [ClientId], [AccessoriesId], [Count], [Cost], [Date_order], [Status_order]) VALUES (2, 2, 3, N'1 штука', 2000.0000, CAST(N'2023-05-01' AS Date), N'Получен')
SET IDENTITY_INSERT [dbo].[OrderAccessories] OFF
GO
SET IDENTITY_INSERT [dbo].[Repairs] ON 

INSERT [dbo].[Repairs] ([Id], [DeviceId], [StaffId], [Date_of_admission], [End_date], [Cost], [Description_of_problem], [Descriprion_of_work_done], [Status]) VALUES (1, 2, 1, CAST(N'2023-05-01' AS Date), CAST(N'2023-05-04' AS Date), 2000.0000, N'Принтер не включается.', N'Замена и перепайка внутренних микросхем.', N'Выполнен')
INSERT [dbo].[Repairs] ([Id], [DeviceId], [StaffId], [Date_of_admission], [End_date], [Cost], [Description_of_problem], [Descriprion_of_work_done], [Status]) VALUES (2, 3, 4, CAST(N'2023-04-20' AS Date), CAST(N'2023-04-21' AS Date), 1200.0000, N'Ноутбук сильно греется при работе.', N'Чистка устройства.', N'Выполнен')
INSERT [dbo].[Repairs] ([Id], [DeviceId], [StaffId], [Date_of_admission], [End_date], [Cost], [Description_of_problem], [Descriprion_of_work_done], [Status]) VALUES (3, 4, 3, CAST(N'2023-10-04' AS Date), CAST(N'2023-03-15' AS Date), 3100.0000, N'Принтер плохо печатает.', N'Замена картриджа.', N'Выполнен')
INSERT [dbo].[Repairs] ([Id], [DeviceId], [StaffId], [Date_of_admission], [End_date], [Cost], [Description_of_problem], [Descriprion_of_work_done], [Status]) VALUES (4, 5, 5, CAST(N'2023-05-02' AS Date), CAST(N'2023-05-03' AS Date), 10000.0000, N'Сматртфон не включается.', N'Замена перегоревшей платы на новую.', N'Выполнен')
INSERT [dbo].[Repairs] ([Id], [DeviceId], [StaffId], [Date_of_admission], [End_date], [Cost], [Description_of_problem], [Descriprion_of_work_done], [Status]) VALUES (5, 1, 4, CAST(N'2023-04-20' AS Date), CAST(N'2023-04-21' AS Date), 3000.0000, N'Ноутбук очень сильно греется при работе.', N'Чистка устройства.', N'Выполнен')
SET IDENTITY_INSERT [dbo].[Repairs] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Id], [Role], [Description]) VALUES (1, N'Клиент', NULL)
INSERT [dbo].[Roles] ([Id], [Role], [Description]) VALUES (2, N'Сотрудник', N'Ремонтник')
INSERT [dbo].[Roles] ([Id], [Role], [Description]) VALUES (3, N'Администратор', NULL)
INSERT [dbo].[Roles] ([Id], [Role], [Description]) VALUES (4, N'Отдел кадров', NULL)
INSERT [dbo].[Roles] ([Id], [Role], [Description]) VALUES (5, N'Ресепшен', NULL)
INSERT [dbo].[Roles] ([Id], [Role], [Description]) VALUES (6, N'Менеджер', NULL)
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Staff] ON 

INSERT [dbo].[Staff] ([Id], [Name], [Surname], [Patronymic], [Experiance], [Post], [Salary], [Date_of_employment], [Photo], [RoleId], [Login], [Password]) VALUES (1, N'Иван', N'Калинин', N'Сергеевич', 8, N'Электрик', 34000.0000, CAST(N'2021-04-01' AS Date), N'Калинин.jpg', 2, N'electrik228', N'S@f3Gu@rd$H@v3n_2024')
INSERT [dbo].[Staff] ([Id], [Name], [Surname], [Patronymic], [Experiance], [Post], [Salary], [Date_of_employment], [Photo], [RoleId], [Login], [Password]) VALUES (2, N'Валентин', N'Ермаков', N'Андреевич', 12, N'Техник', 50000.0000, CAST(N'2020-09-08' AS Date), N'Ермаков.jpg', 2, N'Ermak123', N'CrypT1c@1M#Ke3per!')
INSERT [dbo].[Staff] ([Id], [Name], [Surname], [Patronymic], [Experiance], [Post], [Salary], [Date_of_employment], [Photo], [RoleId], [Login], [Password]) VALUES (3, N'Антон', N'Хомяков', N'Олегович', 10, N'Техник', 50000.0000, CAST(N'2023-03-06' AS Date), N'Хомяков.jpg', 2, N'dgungarik', N'ShieldSw0rd$Defend_88')
INSERT [dbo].[Staff] ([Id], [Name], [Surname], [Patronymic], [Experiance], [Post], [Salary], [Date_of_employment], [Photo], [RoleId], [Login], [Password]) VALUES (4, N'Генадий', N'Авдеев', N'Дмитриевич', NULL, N'Стажёр', 15000.0000, CAST(N'2023-05-06' AS Date), N'Авдеев.jpg', 2, N'AGD', N'F0rtre$$F0rg3@t@ll_2040')
INSERT [dbo].[Staff] ([Id], [Name], [Surname], [Patronymic], [Experiance], [Post], [Salary], [Date_of_employment], [Photo], [RoleId], [Login], [Password]) VALUES (5, N'Глеб', N'Задирако', N'Михайлович', 12, N'Электрик', 35000.0000, CAST(N'2023-05-09' AS Date), N'Задирако.jpg', 2, N'GridiGleb', N'Guard!@n$OfW0rld$789')
INSERT [dbo].[Staff] ([Id], [Name], [Surname], [Patronymic], [Experiance], [Post], [Salary], [Date_of_employment], [Photo], [RoleId], [Login], [Password]) VALUES (6, N'Олег', N'Касаткин', N'Андреевич', 3, N'Программист', 99999.0000, CAST(N'2020-09-01' AS Date), NULL, 3, N'Admin', N'admin')
INSERT [dbo].[Staff] ([Id], [Name], [Surname], [Patronymic], [Experiance], [Post], [Salary], [Date_of_employment], [Photo], [RoleId], [Login], [Password]) VALUES (7, N'Людмила', N'Фокина', N'Васильевна', 5, N'Менеджер по персоналу', 35000.0000, CAST(N'2024-03-07' AS Date), N'Фокина.jpg', 4, N'LudmilaF', N'4$gT$2a!P@b#')
INSERT [dbo].[Staff] ([Id], [Name], [Surname], [Patronymic], [Experiance], [Post], [Salary], [Date_of_employment], [Photo], [RoleId], [Login], [Password]) VALUES (8, N'Константин', N'Лебедев', N'Александрович', 3, N'Ресепшеонист', 23000.0000, CAST(N'2023-12-14' AS Date), N'Лебедев.jpg', 5, N'KLebedev23', N'J9#mN!6%qR*3')
INSERT [dbo].[Staff] ([Id], [Name], [Surname], [Patronymic], [Experiance], [Post], [Salary], [Date_of_employment], [Photo], [RoleId], [Login], [Password]) VALUES (9, N'Ирина', N'Беляева', N'Сергеевна', 3, N'Менеджер', 21000.0000, CAST(N'2023-02-22' AS Date), N'Беляева.jpg', 6, N'IBelyaeva', N'3tG!7P$w9Q')
SET IDENTITY_INSERT [dbo].[Staff] OFF
GO
SET IDENTITY_INSERT [dbo].[Suppliers] ON 

INSERT [dbo].[Suppliers] ([Id], [Company_name], [Contact_person], [Phone_number], [Address], [Email]) VALUES (1, N'MARVEL Дистрибуция', N'Рузаев Андрей Викторович', N'+7 (978) 876-23-41', N'г. Москва, ул. Краснобогатырская, д. 89 стр. 1', N'info@marvel.ru')
INSERT [dbo].[Suppliers] ([Id], [Company_name], [Contact_person], [Phone_number], [Address], [Email]) VALUES (2, N'ARIAT TECH', N'Мамаев Вадим Никитич', N'+7 (954) 632-71-23', N'г. Москва ул. Евремова д.12', N'Info@ariat-tech.com')
SET IDENTITY_INSERT [dbo].[Suppliers] OFF
GO
ALTER TABLE [dbo].[Accessories]  WITH CHECK ADD  CONSTRAINT [FK_Accessories_Suppliers] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Suppliers] ([Id])
GO
ALTER TABLE [dbo].[Accessories] CHECK CONSTRAINT [FK_Accessories_Suppliers]
GO
ALTER TABLE [dbo].[Clients]  WITH CHECK ADD  CONSTRAINT [FK_Clients_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[Clients] CHECK CONSTRAINT [FK_Clients_Roles]
GO
ALTER TABLE [dbo].[Devices]  WITH CHECK ADD  CONSTRAINT [FK_Devices_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO
ALTER TABLE [dbo].[Devices] CHECK CONSTRAINT [FK_Devices_Clients]
GO
ALTER TABLE [dbo].[OrderAccessories]  WITH CHECK ADD  CONSTRAINT [FK_OrderAccessories_Accessories] FOREIGN KEY([AccessoriesId])
REFERENCES [dbo].[Accessories] ([Id])
GO
ALTER TABLE [dbo].[OrderAccessories] CHECK CONSTRAINT [FK_OrderAccessories_Accessories]
GO
ALTER TABLE [dbo].[OrderAccessories]  WITH CHECK ADD  CONSTRAINT [FK_OrderAccessories_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO
ALTER TABLE [dbo].[OrderAccessories] CHECK CONSTRAINT [FK_OrderAccessories_Clients]
GO
ALTER TABLE [dbo].[Repairs]  WITH CHECK ADD  CONSTRAINT [FK_Repairs_Devices] FOREIGN KEY([DeviceId])
REFERENCES [dbo].[Devices] ([Id])
GO
ALTER TABLE [dbo].[Repairs] CHECK CONSTRAINT [FK_Repairs_Devices]
GO
ALTER TABLE [dbo].[Repairs]  WITH CHECK ADD  CONSTRAINT [FK_Repairs_Staff] FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([Id])
GO
ALTER TABLE [dbo].[Repairs] CHECK CONSTRAINT [FK_Repairs_Staff]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FK_Staff_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_Staff_Roles]
GO
USE [master]
GO
ALTER DATABASE [Repair_service] SET  READ_WRITE 
GO
