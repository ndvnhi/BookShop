use BookDB
go

create table Invoice (
	id int primary key identity(1,1),
	customer_name nvarchar(255),
	address nvarchar(255),
	phone nvarchar(10),
	created_date datetime,
	totalPrice float
	)
go

insert into Invoice values ('Alex','New York','0123456789','2024-01-01',20.99);
insert into Invoice values ('Brian','Chiacago','0123456798','2024-01-01',20.99);
insert into Invoice values ('C','US','0123456798','2024-01-01',20.99);
insert into Invoice values ('D','US','0123456798','2024-01-01',20.99);
insert into Invoice values ('E','US','0123456798','2024-01-01',20.99);
insert into Invoice values ('F','US','0123456798','2024-01-01',20.99);
insert into Invoice values ('G','US','0123456798','2024-01-01',20.99);
insert into Invoice values ('H','US','0123456798','2024-01-01',20.99);
insert into Invoice values ('I','US','0123456798','2024-01-01',20.99);
insert into Invoice values ('K','US','0123456798','2024-01-01',20.99);
insert into Invoice values ('L','US','0123456798','2024-01-01',20.99);
insert into Invoice values ('M','US','0123456798','2024-01-01',20.99);
insert into Invoice values ('N','US','0123456798','2024-01-01',20.99);
insert into Invoice values ('P','US','0123456798','2024-01-01',20.99);
insert into Invoice values ('Q','US','0123456798','2024-01-01',20.99);
insert into Invoice values ('MT','US','0123456798','2024-01-01',20.99);


create table Invoice_Detail (
	invoice_id int,
	book_id int,
	amount int,
	foreign key (invoice_id) references Invoice(id),
    foreign key (book_id) references MoreBook(id)
	)
go

insert into Invoice_Detail values (1,1,20.99);
insert into Invoice_Detail values (2,1,20.99);
insert into Invoice_Detail values (3,1,20.99);
insert into Invoice_Detail values (4,1,20.99);
insert into Invoice_Detail values (5,1,20.99);
insert into Invoice_Detail values (6,1,20.99);
insert into Invoice_Detail values (7,1,20.99);
insert into Invoice_Detail values (8,1,20.99);
insert into Invoice_Detail values (9,1,20.99);
insert into Invoice_Detail values (10,1,20.99);
insert into Invoice_Detail values (11,1,20.99);
insert into Invoice_Detail values (13,1,20.99);
insert into Invoice_Detail values (14,1,20.99);
insert into Invoice_Detail values (15,1,20.99);
insert into Invoice_Detail values (16,1,20.99);
insert into Invoice_Detail values (17,1,20.99);
