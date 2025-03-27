create database Carrental

use carrental

create table vehicle(
car_id int primary key identity(1,1) not null,
make VARCHAR(20),
model varchar(20),
year int not null,
daily_rate decimal(10,2),
status varchar(20) check(status in('available','not available')),
passenger_cap int,
engine_cap int
);
select * from vehicle

insert into vehicle (make,model,year,daily_rate,status,
passenger_cap,engine_cap) values
('toyata','camry',2022,50.00,'available',4,1450)
insert into vehicle (make,model,year,daily_rate,status,
passenger_cap,engine_cap) values
('honda','civic',2023,45.00,'available',7,1500),
('ford','focus',2022,48.00,'not available',4,1400),
('nissan','altima',2023,52.00,'available',7,1200),
('cheverolet','malibu',2022,47.00,'available',4,1800),
('hyundai','sonata',2023,49.00,'not available',7,1400),
('bmw','3 series',2023,60.00,'available',7,2499),
('mercedes','c-class',2022,58.00,'available',8,2599),
('audi','a4',2022,55.00,'not available',4,2500),
('lexus','es',2023,54.00,'available',4,2500);

select * from vehicle


create table customer(
customer_id int primary key identity(1,1),
first_name varchar(20) not null,
last_name varchar(20),
email varchar(30) unique not null,
phone_number bigint unique not null
);

insert into customer (first_name,last_name,email,phone_number)
values
('John','Doe','johndoe@example.com',555-555-5555),
('Jane','Smith','janesmith@example.com', 555-123-4567),
('Robert','Johnson', 'robert@example.com', 555-789-1234),
('Sarah','Brown', 'sarah@example.com ',555-456-7890),
('David','Lee', 'david@example.com', 555-987-6543),
('Laura','Hall' ,'laura@example.com ',555-234-5678),
('Michael','Davis','michael@example.com', 555-876-5432),
('Emma','Wilson','emma@example.com', 555-432-1098),
('William','Taylor','william@example.com', 555-321-6547),
 ('Olivia','Adams','olivia@example.com ',555-765-4321);

 select * from customer

create table lease(
lease_id int primary key identity(1,1),
car_id int,
customer_id int,
start_date date,
end_date date ,
lease_type varchar(20) check(lease_type in('daily','monthly')),
foreign key (car_id) references vehicle (car_id),
foreign key (customer_id) references customer(customer_id)
);

INSERT INTO Lease (car_id, customer_id, start_date, end_date,lease_type)  
VALUES  
(1, 1, '2023-01-01', '2023-01-05','daily'), 
INSERT INTO Lease (car_id, customer_id, start_date, end_date,lease_type)  
VALUES  
(2, 2, '2023-02-15',' 2023-02-28','monthly'),  
(3, 3, '2023-03-10',' 2023-03-15','daily'),  
(4, 4, '2023-04-20', '2023-04-30','daily'),  
(5, 5, '2023-05-05',' 2023-05-10','monthly'),  
(4, 3, '2023-06-15',' 2023-06-30','daily'),  
(7, 7, '2023-07-01',' 2023-07-10','daily'),  
(8, 8, '2023-08-12',' 2023-08-15','monthly'),  
(3, 3, '2023-09-07', '2023-09-10','monthly'),  
(10, 10, '2023-10-10','2023-10-31','daily'), 
(2,2,'2025-03-01','2025-03-10','Daily'),
(5,2,'2025-03-05','2025-03-20','Monthly'),
(7,2,'2025-03-08','2025-03-15','Daily');

select * from lease


create table payment(
payment_id int primary key identity(1,1) not null,
lease_id int,
payment_date date not null,
amount decimal(10,2) not null,
foreign key (lease_id) references lease(lease_id)
);

INSERT INTO Payment (lease_id, payment_date, amount)  
VALUES  
(3, '2023-01-03', 200.00),  
(4, '2023-02-20', 1000.00),  
(5, '2023-03-12', 75.00),  
(6, '2023-04-25', 900.00),  
(7, '2023-05-07', 60.00),  
(8, '2023-06-18', 1200.00),  
(9, '2023-07-03', 40.00),  
(10, '2023-08-14', 1100.00),  
(11, '2023-09-09', 80.00),  
(12, '2023-10-25', 1500.00),  
(13, '2025-03-02', 250.00),  
(14, '2025-03-06', 300.00),  
(15, '2025-03-10', 500.00);

select * from payment

--1. Update the daily rate for a Mercedes car to 68.
update vehicle
set daily_rate=68.00
where make='mercedes'

--2. Delete a specific customer and 
--all associated leases and payments.

--since its a child table of customer altered to cascade
alter table Lease
ADD constraint FK_lease_customer
foreign key (customer_id) 
references customer(customer_id) ON DELETE CASCADE;

alter table payment
add constraint FK_payment_lease 
foreign key(lease_id) 
references lease(lease_id) ON DELETE CASCADE;

delete from customer where customer_id = 6;

--3.Rename the "paymentDate" column in the Payment table to "transactionDate".

exec sp_rename 'payment.payment_date','transaction_date'

select * from payment

--Find a specific customer by email.
select first_name,last_name
from customer
where email='janesmith@example.com'

select * from customer

select * from lease

--5. Get active leases for a specific customer.

INSERT INTO Lease (car_id, customer_id, start_date, end_date,lease_type)  
VALUES  
(3, 4, '2025-03-01', '2025-03-30','daily')

SELECT * FROM Lease
WHERE customer_id = 4 AND end_date >= GETDATE();

--6. Find all payments made by a customer with a specific phone number.

INSERT INTO Payment (lease_id,transaction_date, amount)  
VALUES  
(3, '2023-02-03', 200.00) 

select p.* 
from payment p
join lease l on l.lease_id=p.lease_id
join customer c on c.customer_id=l.customer_id
where c.phone_number= 555-555-5555;

--7. Calculate the average daily rate of all available cars

select avg(daily_rate) as average
from vehicle
where status='available'

--8. Find the car with the highest daily rate

select make,model
from vehicle 
where daily_rate=(select max(daily_rate) from vehicle)

--Retrieve all cars leased by a specific customer
select * from lease
select * from vehicle

INSERT INTO Lease (car_id, customer_id, start_date, end_date,lease_type)  
VALUES  
(3, 1, '2025-03-01', '2025-03-30','daily')

select * 
from vehicle v
join lease l on l.car_id=v.car_id
where customer_id=1

-- Find the details of the most recent lease
select *
from lease
order by start_date desc

--List all payments made in the year 2023
select *
from payment
where year(transaction_date)=2023


--12. Retrieve customers who have not made any payments

select *
from customer c
where not exists (
    select * 
	from lease l
    join payment p on l.lease_ID = p.lease_ID
    where l.customer_id = c.customer_id
);

--Retrieve Car Details and Their Total Payments

select * from vehicle
select * from lease
select * from payment

select v.car_id,v.model,sum(p.amount) as total
from vehicle v
join lease l on l.car_id=v.car_id
join payment p on p.lease_id=l.lease_id
group by v.car_id,v.model

--Calculate Total Payments for Each Customer

select c.customer_id,sum(p.amount) as total
from customer c
join lease l on l.customer_id=c.customer_id
join payment p on p.lease_id=l.lease_id
group by c.customer_id;

--. List Car Details for Each Lease

select l.lease_id,l.lease_type,v.make,v.model
from lease l
join vehicle v on v.car_id=l.car_id
group by l.lease_id,l.lease_type,v.make,v.model;


-- Retrieve Details of Active Leases with Customer and Car Information
select l.*
from lease l                                      --doubt
join customer c on c.customer_id=l.car_id
join vehicle v on v.car_id=l.car_id
where end_date >=getdate();

-- Find the Customer Who Has Spent the Most on Leases
select c.first_name,sum(p.amount) as total
from customer c
join lease l on l.customer_id=c.customer_id
join payment p on l.lease_id=p.lease_id
group by c.first_name
order by total desc

--. List All Cars with Their Current Lease Information

select v.make,v.model,l.lease_id,l.lease_type
from vehicle v
join lease l on l.car_id=v.car_id