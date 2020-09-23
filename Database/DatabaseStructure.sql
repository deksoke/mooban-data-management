use MoobanDB;

CREATE TABLE Roles (
	Id int(6) unsigned auto_increment Primary key,
	Name varchar(100) NULL
	);
	

CREATE TABLE Settings (
	Id int(6) unsigned auto_increment Primary key,
	ThemeName varchar(200) not NULL
	);
	
CREATE TABLE UserClaims (
	Id int(6) unsigned auto_increment Primary key,
	UserId int(6) not null,
	ClaimType varchar(200) not null,
	ClaimValue varchar(200) not null
	);
	
CREATE TABLE UserPhotos (
	Id int(6),
	Image varchar(4000) not null
	);
	
CREATE TABLE Users(
	Id int(6) unsigned auto_increment primary key,
	Login varchar(2000) NOT NULL,
	Password varchar(2000) NULL,
	FirstName varchar(2000) NULL,
	LastName varchar(2000) NULL,
	Email varchar(2000) NOT NULL,
	Age int(6) NULL,
	Street varchar(2000) NULL,
	City varchar(2000) NULL,
	ZipCode varchar(2000) NULL,
	Lat float NULL,
	Lng float NULL,
	IsDeleted bit NOT NULL
);