USE [BookDB]
GO
/****** Object:  User [admin]    Script Date: 1/5/2024 8:43:07 AM ******/
CREATE USER [admin] FOR LOGIN [admin] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [admin001]    Script Date: 1/5/2024 8:43:07 AM ******/
CREATE USER [admin001] FOR LOGIN [admin001] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [librarian]    Script Date: 1/5/2024 8:43:07 AM ******/
CREATE USER [librarian] FOR LOGIN [librarian] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 1/5/2024 8:43:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 1/5/2024 8:43:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoice](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[customer_name] [nvarchar](255) NULL,
	[address] [nvarchar](255) NULL,
	[phone] [nvarchar](10) NULL,
	[created_date] [datetime] NULL,
	[totalPrice] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoice_Detail]    Script Date: 1/5/2024 8:43:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoice_Detail](
	[invoice_id] [int] NULL,
	[book_id] [int] NULL,
	[amount] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MoreBook]    Script Date: 1/5/2024 8:43:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MoreBook](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
	[cover_image] [nvarchar](100) NULL,
	[author] [nvarchar](100) NULL,
	[year] [int] NULL,
	[price] [float] NULL,
	[category_id] [int] NULL,
	[quantity] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([id], [name]) VALUES (1, N'Classic Literature')
INSERT [dbo].[Category] ([id], [name]) VALUES (2, N'Fiction')
INSERT [dbo].[Category] ([id], [name]) VALUES (3, N'Fantasy')
INSERT [dbo].[Category] ([id], [name]) VALUES (4, N'Romance')
INSERT [dbo].[Category] ([id], [name]) VALUES (5, N'Mystery')
INSERT [dbo].[Category] ([id], [name]) VALUES (6, N'Science Fiction')
INSERT [dbo].[Category] ([id], [name]) VALUES (7, N'Comedy')
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Invoice] ON 

INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (1, N'Alex', N'New York', N'0123456789', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (2, N'Brian', N'Chiacago', N'0123456798', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (3, N'C', N'US', N'0123456798', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (4, N'D', N'US', N'0123456798', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (5, N'E', N'US', N'0123456798', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (6, N'F', N'US', N'0123456798', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (7, N'G', N'US', N'0123456798', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (8, N'H', N'US', N'0123456798', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (9, N'I', N'US', N'0123456798', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (10, N'K', N'US', N'0123456798', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (11, N'L', N'US', N'0123456798', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (13, N'M', N'US', N'0123456798', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (14, N'N', N'US', N'0123456798', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (15, N'P', N'US', N'0123456798', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (16, N'Q', N'US', N'0123456798', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
INSERT [dbo].[Invoice] ([id], [customer_name], [address], [phone], [created_date], [totalPrice]) VALUES (17, N'MT', N'US', N'0123456798', CAST(N'2024-01-01T00:00:00.000' AS DateTime), 20.99)
SET IDENTITY_INSERT [dbo].[Invoice] OFF
GO
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (1, 1, 20)
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (2, 1, 20)
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (3, 1, 20)
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (4, 1, 20)
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (5, 1, 20)
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (6, 1, 20)
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (7, 1, 20)
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (8, 1, 20)
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (9, 1, 20)
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (10, 1, 20)
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (11, 1, 20)
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (13, 1, 20)
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (14, 1, 20)
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (15, 1, 20)
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (16, 1, 20)
INSERT [dbo].[Invoice_Detail] ([invoice_id], [book_id], [amount]) VALUES (17, 1, 20)
GO
SET IDENTITY_INSERT [dbo].[MoreBook] ON 

INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (1, N'Anne of Green Gables', N'assets/Book/01.jpg', N'Lucy Maud Montgomery', 1908, 20.99, 1, 30)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (2, N'To Kill a Mockingbird', N'assets/Book/02.jpg', N'Harper Lee', 1960, 15.99, 2, 11)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (3, N'1984', N'assets/Book/03.jpg', N'George Orwell', 1949, 12.99, 2, 0)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (4, N'The Great Gatsby', N'assets/Book/04.jpg', N'F. Scott Fitzgerald', 1925, 18.99, 2, 2)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (5, N'The Catcher in the Rye', N'assets/Book/05.jpg', N'J.D. Salinger', 1951, 14.99, 2, 1)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (6, N'Harry Potter and the Sorcerer''s Stone', N'assets/Book/06.jpg', N'J.K. Rowling', 1997, 24.99, 3, 5)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (7, N'The Lord of the Rings: The Fellowship of the Ring', N'assets/Book/07.jpg', N'J.R.R. Tolkien', 1954, 22.99, 3, 8)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (8, N'Pride and Prejudice', N'assets/Book/08.jpg', N'Jane Austen', 1813, 16.99, 4, 42)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (9, N'The Da Vinci Code', N'assets/Book/09.jpg', N'Dan Brown', 2003, 19.99, 5, 31)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (10, N'The Hitchhiker''s Guide to the Galaxy', N'assets/Book/10.jpg', N'Douglas Adams', 1979, 17.99, 6, 12)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (11, N'The Hobbit', N'assets/Book/11.jpg', N'J.R.R. Tolkien', 1937, 21.99, 3, 8)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (12, N'Jane Eyre', N'assets/Book/12.jpg', N'Charlotte Brontë', 1847, 16.99, 1, 35)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (13, N'The Girl with the Dragon Tattoo', N'assets/Book/13.jpg', N'Stieg Larsson', 2005, 18.99, 5, 27)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (14, N'The Martian', N'assets/Book/14.jpg', N'Andy Weir', 2011, 20.99, 6, 24)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (15, N'Wuthering Heights', N'assets/Book/15.jpg', N'Emily Brontë', 1847, 17.99, 1, 47)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (16, N'The Shining', N'assets/Book/16.jpg', N'Stephen King', 1977, 19.99, 5, 50)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (17, N'Sense and Sensibility', N'assets/Book/17.jpg', N'Jane Austen', 1811, 15.99, 4, 93)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (18, N'Brave New World', N'assets/Book/18.jpg', N'Aldous Huxley', 1932, 22.99, 2, 99)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (19, N'The Alchemist', N'assets/Book/19.jpg', N'Paulo Coelho', 1988, 14.99, 2, 39)
INSERT [dbo].[MoreBook] ([id], [name], [cover_image], [author], [year], [price], [category_id], [quantity]) VALUES (20, N'The Importance of Being Earnest', N'assets/Book/20.jpg', N'Oscar Wilde', 1895, 16.99, 7, 1)
SET IDENTITY_INSERT [dbo].[MoreBook] OFF
GO
ALTER TABLE [dbo].[Invoice_Detail]  WITH CHECK ADD FOREIGN KEY([book_id])
REFERENCES [dbo].[MoreBook] ([id])
GO
ALTER TABLE [dbo].[Invoice_Detail]  WITH CHECK ADD FOREIGN KEY([invoice_id])
REFERENCES [dbo].[Invoice] ([id])
GO
ALTER TABLE [dbo].[MoreBook]  WITH CHECK ADD  CONSTRAINT [FK_MoreBook_Category] FOREIGN KEY([category_id])
REFERENCES [dbo].[Category] ([id])
GO
ALTER TABLE [dbo].[MoreBook] CHECK CONSTRAINT [FK_MoreBook_Category]
GO
