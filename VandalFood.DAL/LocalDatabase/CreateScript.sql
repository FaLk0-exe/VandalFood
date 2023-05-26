CREATE TABLE OrderStatuses(
Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
Title NVARCHAR(32) Collate Cyrillic_General_CI_AS NOT NULL
);

CREATE TABLE ContactTypes(
Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
Title NVARCHAR(32) Collate Cyrillic_General_CI_AS NOT NULL
);

CREATE TABLE RoleTypes(
Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
Title NVARCHAR(32) Collate Cyrillic_General_CI_AS NOT NULL
);


CREATE TABLE ProductTypes(
Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
Title NVARCHAR(32) Collate Cyrillic_General_CI_AS NOT NULL
);

CREATE TABLE Operators(
Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
[Login] NVARCHAR(32) Collate Cyrillic_General_CI_AS NOT NULL,
[Password] NVARCHAR(32) Collate Cyrillic_General_CI_AS NOT NULL,
LeftName NVARCHAR(64) Collate Cyrillic_General_CI_AS NOT NULL,
RightName NVARCHAR(64) Collate Cyrillic_General_CI_AS NOT NULL,
RoleTypeId INT NOT NULL,
FOREIGN KEY(RoleTypeId) REFERENCES RoleTypes(Id)
);

CREATE TABLE Customers(
Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
[Login] NVARCHAR(32) Collate Cyrillic_General_CI_AS NOT NULL,
[Password] NVARCHAR(32) Collate Cyrillic_General_CI_AS NOT NULL,
LeftName NVARCHAR(64) Collate Cyrillic_General_CI_AS NOT NULL
);

CREATE TABLE Products(
Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
IsActive BIT NOT NULL DEFAULT 1,
Title NVARCHAR(64) Collate Cyrillic_General_CI_AS NOT NULL,
[Description] NVARCHAR(256) Collate Cyrillic_General_CI_AS NOT NULL DEFAULT('-'),
[Weight] INTEGER NOT NULL DEFAULT 0,
Price Decimal NOT NULL DEFAULT 0,
ProductTypeId INT NOT NULL,
FOREIGN KEY (ProductTypeId) REFERENCES ProductTypes(Id));

CREATE TABLE CustomerContacts(
CustomerId INT NOT NULL,
ContactTypeId INT NOT NULL,
Primary key(CustomerId,ContactTypeId),
[Value] NVARCHAR(128) Collate Cyrillic_General_CI_AS NOT NULL,
FOREIGN KEY(CustomerId) REFERENCES Customers(Id),
FOREIGN KEY (ContactTypeId) REFERENCES ContactTypes(Id)
);

CREATE TABLE CustomerOrders(
Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
OperatorId INT NOT NULL,
OrderStatusId INT NOT NULL,
OrderDate DateTime NOT NULL DEFAULT GETDATE(),
CustomerName NVARCHAR(32) Collate Cyrillic_General_CI_AS NOT NULL,
FOREIGN KEY(OperatorId) REFERENCES Operators(Id),
FOREIGN KEY (OrderStatusId) REFERENCES OrderStatuses(Id)
);

CREATE TABLE OrderContacts(
CustomerOrderId INT NOT NULL ,
ContactTypeId INT NOT NULL,
Primary key(CustomerOrderId,ContactTypeId),
[Value] NVARCHAR(128) Collate Cyrillic_General_CI_AS NOT NULL,
FOREIGN KEY(CustomerOrderId) REFERENCES CustomerOrders(Id),
FOREIGN KEY (ContactTypeId) REFERENCES ContactTypes(Id)
);

CREATE TABLE OrderItems(
ProductId INT NOT NULL,
CustomerOrderId INT NOT NULL,
Primary key(CustomerOrderId,ProductId),
Amount INT NOT NULL DEFAULT 1,
Price DECIMAL NOT NULL DEFAULT 0
FOREIGN KEY(ProductId) REFERENCES Products(Id),
FOREIGN KEY (CustomerOrderId) REFERENCES CustomerOrders(Id)
);

