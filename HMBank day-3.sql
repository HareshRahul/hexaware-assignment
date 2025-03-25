 --DAY_1
 create database HMBANK;

use hmbank;

create table customer (
 customer_id int identity(1,1) primary key,
 first_name varchar(30),
 last_name varchar(30),
 dob date not null,
 email varchar(30) unique not null,
 phone_number bigint unique not null, -- since the int datatype is limited i changed to bigint to support larger phone numbers
 address varchar(255)
);

create table accounts (
 account_id int primary key,
 customer_id int not null,
 account_type varchar(20) check (account_type in ('savings', 'current', 'zero_balance')),
 balance decimal(15,2) check (balance >= 0),
 foreign key (customer_id) references customer(customer_id)
);

create table transactions (
 transaction_id int primary key,
 account_id int not null,
 transaction_type varchar(20) check (transaction_type in ('deposit', 'withdrawal', 'transfer')),
 amount decimal(15,2) check (amount >= 1),
 foreign key (account_id) references accounts(account_id)
);

---------DAY-2---------------------------------------------------------------------------------------------

insert into customer (first_name, last_name, dob, email, phone_number, address) values
('walter','white','1990-05-14','white@example.com',9876543210,'chennai')
insert into customer (first_name, last_name, dob, email, phone_number, address) values
('jesse','pinkman','1984-09-24','jessepinkman@gmail.com',9876543211,'coimbatore'),
('saul','goodman','1970-11-12','saulgoodman@gmail.com',9876543212,'madurai'),
('gus','fring','1958-02-25','gusfring@gmail.com',9876543213,'trichy'),
('hank','schrader','1966-03-27','hankschrader@gmail.com',9876543214,'salem'),
('skyler','white','1970-08-11','skylerwhite@gmail.com',9876543215,'erode'),
('mike','ehrmantraut','1947-04-04','mikeehrmantraut@gmail.com',9876543216,'vellore'),
('heisenberg','unknown','1960-01-20','heisenberg@gmail.com',9876543217,'thanjavur'),
('tony','stark','1970-05-29','tonystark@gmail.com',9876543218,'tirunelveli'),
('steve','rogers','1918-07-04','steverogers@gmail.com',9876543219,'thoothukudi'),
('bruce','wayne','1939-04-17','brucewayne@gmail.com',9876543220,'karur'),
('peter','parker','2001-08-10','peterparker@gmail.com',9876543221,'dindigul'),
('joey','tribbiani','1968-01-09','joeytribbiani@gmail.com',9876543222,'namakkal'),
('ross','geller','1967-10-18','rossgeller@gmail.com',9876543223,'cuddalore'),
('monica','geller','1969-04-22','monicageller@gmail.com',9876543224,'tiruvannamalai'),
('rachel','green','1969-05-05','rachelgreen@gmail.com',9876543225,'kanyakumari'),
('chandler','bing','1968-04-08','chandlerbing@gmail.com',9876543226,'kumbakonam'),
('phoebe','buffay','1967-02-16','phoebebuffay@gmail.com',9876543227,'ariyalur'),
('ted','mosby','1978-04-25','tedmosby@gmail.com',9876543228,'pudukkottai'),
('virat','kohli','1990-03-04','virat@gmail.com',8876543210,'chennai');

select * from customer

insert into accounts (account_id, customer_id, account_type, balance) values
(101,1,'savings',5000.00),
(102,2,'current',15000.50),
(103,3,'zero_balance',0.00),
(104,4,'savings',7800.75),
(105,5,'current',24000.00),
(106,6,'savings',6000.25),
(107,7,'zero_balance',0.00),
(108,8,'current',18000.00),
(109,9,'savings',8200.50),
(110,10,'savings',9500.75),
(111,11,'zero_balance',0.00),
(112,12,'current',11200.00),
(113,13,'savings',7300.30),
(114,14,'savings',9200.90),
(115,15,'zero_balance',0.00),
(116,16,'current',26000.25),
(117,17,'savings',5800.80),
(118,18,'savings',7100.45),
(119,19,'current',19500.00),
(120,21,'savings',00.00)

select * from accounts


insert into transactions (transaction_id, account_id, transaction_type, amount) values
(1,101,'deposit',2000.00),
(2,102,'withdrawal',500.00),
(3,103,'deposit',1000.00),
(4,104,'withdrawal',1200.00),
(5,105,'deposit',5000.00),
(6,106,'deposit',2500.00),
(7,107,'deposit',800.00),
(8,108,'withdrawal',3000.00),
(9,109,'deposit',2200.50),
(10,110,'withdrawal',750.00),
(11,111,'deposit',1500.00),
(12,112,'transfer',2000.00),
(13,113,'withdrawal',1000.00),
(14,114,'deposit',3000.00),
(15,115,'deposit',700.00),
(16,116,'withdrawal',4000.00),
(17,117,'deposit',3500.00),
(18,118,'withdrawal',600.00),
(19,119,'transfer',5000.00);


select * from transactions


----1 query to retrieve the name, account type and email of all customers.  (documentation refered)
select cus.first_name, cus.last_name, acc.account_type,cus.email
from customer cus join accounts acc on cus.customer_id = acc.customer_id;

------------2 query to list all transaction corresponding customer.(documentation refered)
select cus.first_name, cus.last_name, trans.transaction_id, trans.account_id, trans.transaction_type, trans.amount
from customer cus join accounts acc on cus.customer_id = acc.customer_id join transactions trans on acc.account_id =trans.account_id;

-----3  query to increase the balance of a specific account by a certain amount.
update accounts set balance=5000+balance where account_id=102;

----4 query to Combine first and last names of customers as a full_name.
select concat(first_name,' ',last_name) as full_name from customer;

---5 query to remove accounts with a balance of zero where the account type is savings.
delete from accounts where balance=0 and account_type ='savings';

---6  query to Find customers living in a specific city.
select * from customer where address='madurai';

-----7 query to Get the account balance for a specific account.
select balance from accounts where account_id=108;

-------8  query to List all current accounts with a balance greater than $1,000.
select * from accounts
where account_type = 'current' and balance > 1000;

----9 query to Retrieve all transactions for a specific account.
select * from transactions where account_id = 111;

------10 query to Calculate the interest accrued on savings accounts based on a given interest rate
select account_id, balance, balance * 0.05 as interest -- change 0.05 to the interest rate
from accounts
where account_type = 'savings';

--------------11 query to Identify accounts where the balance is less than a specified overdraft limit.
select * from accounts
where balance < -5000;

------12 query to Find customers not living in a specific city. 
select * from customer where address != 'chennai'; 



-------------------------------------------------------------------------------
--DAY_3
--find the average account balance for all customers
select avg(balance) as average_balance 
from accounts;

--retrieve the top 10 highest account balances
select top 10 *
from accounts 
order by balance desc;

--calculate total deposits for all customers on a specific date
select sum(amount) as total_deposits from transactions where transaction_type='deposit' and transaction_date='2025-02-25';

select * from customer order by dob asc ;
select * from customer order by dob desc ;

--retrieve transaction details along with the account type
select t.*,a.account_type 
from transactions t 
join accounts a on t.account_id=a.account_id;

--get a list of customers along with their account details

select c.first_name,c.last_name,a.account_id,a.account_type
from customer c
join accounts a on a.customer_id=c.customer_id

---retrieve transaction details along with customer information for a specific account
select c.*,t.transaction_id,t.transaction_type,a.account_id
from customer c
join accounts a on a.customer_id=c.customer_id
join transactions t on t.account_id=a.account_id

--identify customers who have more than one account
select customer_id,count(*) as account_count 
from accounts 
group by customer_id having count(*)>1;
select * from customer

--calculate the difference in transaction amounts between deposits and withdrawals
select account_id,sum(case when transaction_type='deposit' then amount else 0 end)
                  -sum(case when transaction_type='withdrawal'  then amount else 0 end) as balance_diff
from transactions group by account_id;

--calculate the average daily balance for each account over a specified period
--select account_id,avg(balance) as avg_daily_balance 
--from accounts where last_updated between '2023-03-01' and '2025-03-25' group by account_id;

--calculate the total balance for each account type
select account_type,sum(balance) as total_balance
from accounts
group by account_type

--identify accounts with the highest number of transactions order by descending order
select account_id,count(*) as total
from transactions 
group by account_id
order by total desc;

--list customers with high aggregate account balances, along with their account types
select c.customer_id,c.first_name,c.last_name,a.account_type,sum(a.balance) as total_balance 
from customer c 
join accounts a on c.customer_id=a.customer_id 
group by c.customer_id,c.first_name,c.last_name,a.account_type having sum(a.balance)>5000;

--identify and list duplicate transactions based on transaction amount, date, and account
select account_id,amount,transaction_date,count(*) 
from transactions 
group by account_id,amount,transaction_date having count(*)>1;


----------DAY4
--retrieve the customer(s) with the highest account balance

select * from customer 
where customer_id in(select customer_id from accounts where balance=(select max(balance) from accounts));


select * from accounts
insert into accounts (account_id, customer_id, account_type, balance) values
(122,1,'savings',500.00)

insert into accounts (account_id, customer_id, account_type, balance) values  ---inserted for the customer to have two accounts(same customer id)
(101,1,'savings',5000.00),

--calculate the average account balance for customers who have more than one account
select avg(balance) 
from accounts 
where customer_id in(select customer_id from accounts group by customer_id having count(*)>1);

--retrieve accounts with transactions whose amounts exceed the average transaction amount
select * from transactions 
where amount>(select avg(amount) from transactions);

--identify customers who have no recorded transactions
select * from customer
where customer_id not in(select distinct customer_id from accounts a join transactions t on a.account_id=t.account_id);

--calculate the total balance of accounts with no recorded transactions

select sum(balance) 
from accounts 
where account_id not in(select distinct account_id from transactions);

--retrieve transactions for accounts with the lowest balance
select * 
from transactions 
where account_id in(select account_id from accounts where balance=(select min(balance) from accounts));

--.identify customers who have accounts of multiple types
select customer_id,count(distinct account_type) as account_types 
from accounts 
group by customer_id having count(distinct account_type)>1;

--calculate the percentage of each account type out of the total number of accounts
select account_type,count(*)*100.0/(select count(*) from accounts) as percentage 
from accounts group by account_type;

--retrieve all transactions for a customer with a given customer_id
select t.* 
from transactions t 
join accounts a on t.account_id=a.account_id where a.customer_id=101;

--calculate the total balance for each account type, including a subquery within the select clause
select account_type,(select sum(balance)  from accounts a2 where a2.account_type=a1.account_type) as total_balance 
from accounts a1 group by account_type;
