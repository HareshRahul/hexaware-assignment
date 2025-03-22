create database HMBank;

use hmbank;
create table customer(
     customer_id INT identity(1,1) primary key not null,
	 --customer_id int primary key  auto_increment
	 first_name varchar(30),
	 last_name varchar(30),
	 DOB date not null,
	 email varchar(30) unique not null,
	 phone_number int unique not null,
	 address varchar(255)
);

create table accounts(
       account_id int primary key,
	   customer_id int not null
	   foreign key (customer_id) references customer(customer_id) ,
	   account_type varchar(20) check (account_type in ('savings', 'current', 'zero_balance')),
	   balance decimal(15,2) check (balance >= 0),
);


create table transactions(
        transaction_id int primary key,
		account_id int not null,
		foreign key (account_id) references accounts(account_id),
		transaction_type varchar(20) check(transaction_type in ('deposit', 'withdrawal', 'transfer')),
		amount decimal(15,2) check (amount>=1)

);
select * from information_schema.tables where table_name = 'customers';
drop table if exists customers;