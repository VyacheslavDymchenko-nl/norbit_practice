CREATE TABLE Units (
    UnitID   UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    UnitName NVARCHAR (100)   UNIQUE NOT NULL
);

CREATE TABLE Categories (
    CategoryID   UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    CategoryName NVARCHAR (100)   UNIQUE NOT NULL
);

CREATE TABLE Products (
    ProductID   UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    CategoryID  UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Categories (CategoryID),
    UnitID      UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Units (UnitID),
    ProductName NVARCHAR (100)   NOT NULL,
    IsActive    BIT              DEFAULT (1) NOT NULL
);

CREATE TABLE Purchases (
    PurchaseID     UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    ProductID      UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Products (ProductID),
    PurchaseDate   DATETIME         NOT NULL,
    ExpirationDate DATETIME        ,
    QuantityTotal  DECIMAL (10, 3)  NOT NULL,
    PriceTotal     DECIMAL (10, 2)  NOT NULL
);

CREATE TABLE Meals (
    MealID   UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    MealName NVARCHAR (100)  ,
    MealDate DATETIME         NOT NULL
);

CREATE TABLE MealItems (
    MealID       UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Meals (MealID),
    PurchaseID   UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Purchases (PurchaseID),
    QuantityUsed DECIMAL (10, 3)  NOT NULL,
    PRIMARY KEY (MealID, PurchaseID)
);

CREATE UNIQUE INDEX UX_Products_CategoryID_ProductName
    ON Products(CategoryID, ProductName); -- Очень часто будут добавляться покупки, для которых нужно будет быстро находить продукт, на который затем будет ссылаться запись из покупки + продукт в своей категории должен быть уникален  -- (категорий НАМНОГО меньше, чем продуктов). Минусы - добавление/удаление/изменение будет дольше

INSERT  INTO Units (UnitID, UnitName)
VALUES            ('97801dbd-e635-44fe-bdfd-9b4446dc35cc', 'г.'),
('903f4980-8ea5-4ae6-934d-8d23b0515acf', 'мл.'),
('ffc754c1-411f-44a0-8c25-4ea17578d68d', 'шт.');

INSERT  INTO Categories (CategoryID, CategoryName)
VALUES                 ('1bf571bf-56e4-4e18-83f2-2697352b01c5', 'Крупы'),
('b6d77e55-1dd8-479c-b30d-f1d333eb69a8', 'Мясо'),
('5fd8a051-7727-4ee2-96f4-86bd86ff90ec', 'Молочка');

INSERT  INTO Products (ProductID, CategoryID, UnitID, ProductName)
VALUES               ('75875cb2-9d83-4e35-aaab-0e53aea05529', '1bf571bf-56e4-4e18-83f2-2697352b01c5', '97801dbd-e635-44fe-bdfd-9b4446dc35cc', 'Рис'),
('8cf0ca06-a320-46d8-b528-1e13799d8035', 'b6d77e55-1dd8-479c-b30d-f1d333eb69a8', '97801dbd-e635-44fe-bdfd-9b4446dc35cc', 'Курица'),
('9f3ac70d-8fca-4a6b-9f30-a3cf5e53fe7b', '5fd8a051-7727-4ee2-96f4-86bd86ff90ec', '903f4980-8ea5-4ae6-934d-8d23b0515acf', 'Молоко'),
('2f3ac70d-8fca-4a6b-9f30-a3cf5e53fe7b', '5fd8a051-7727-4ee2-96f4-86bd86ff90ec', '903f4980-8ea5-4ae6-934d-8d23b0515acf', 'Кефир');

INSERT  INTO Purchases (ProductID, PurchaseDate, ExpirationDate, QuantityTotal, PriceTotal)
VALUES                ('75875cb2-9d83-4e35-aaab-0e53aea05529', CAST (GETDATE() AS DATETIME), '2027-07-09T14:52:00', 900, 144.99),
('9f3ac70d-8fca-4a6b-9f30-a3cf5e53fe7b', CAST (GETDATE() AS DATETIME), '2026-07-16T14:52:00', 900, 76.49),
('9f3ac70d-8fca-4a6b-9f30-a3cf5e53fe7b', CAST (GETDATE() AS DATETIME), '2026-07-17T14:52:00', 900, 76.49),
('2f3ac70d-8fca-4a6b-9f30-a3cf5e53fe7b', CAST (GETDATE() AS DATETIME), '2026-07-16T14:53:00', 900, 76.49);

SELECT   pr.ProductName AS [Название продукта],
         -- Показать всю молочку, отсортированную по сроку годности по возрастанию (выборка данных с фильтрацией, сортировкой)
c.CategoryName AS [Категория],
         pc.ExpirationDate AS [Срок годности истекает]
FROM     Purchases AS pc
         INNER JOIN
         Products AS pr
         ON pc.ProductID = pr.ProductID
         INNER JOIN
         Categories AS c
         ON pr.CategoryID = c.CategoryID
WHERE    c.CategoryName = 'Молочка'
ORDER BY pc.ExpirationDate ASC;

INSERT  INTO Categories (CategoryID, CategoryName)
VALUES                 ('7bf571bf-56e4-4e18-83f2-2697352b01c5', 'Кисломолочка');

UPDATE Products -- Изменение данных
SET    CategoryID = '7bf571bf-56e4-4e18-83f2-2697352b01c5'
WHERE  ProductName = 'Кефир';

SELECT   pr.ProductName AS [Название продукта],
         c.CategoryName AS [Категория],
         pc.ExpirationDate AS [Срок годности истекает]
FROM     Purchases AS pc
         INNER JOIN
         Products AS pr
         ON pc.ProductID = pr.ProductID
         INNER JOIN
         Categories AS c
         ON pr.CategoryID = c.CategoryID
WHERE    c.CategoryName = 'Молочка'
         OR c.CategoryName = 'Кисломолочка'
ORDER BY pc.ExpirationDate ASC;

DELETE Purchases -- Удаление данных
WHERE  ExpirationDate < '2026-07-17T14:52:00.000';

INSERT  INTO Purchases (ProductID, PurchaseDate, ExpirationDate, QuantityTotal, PriceTotal)
VALUES                ('75875cb2-9d83-4e35-aaab-0e53aea05529', CAST (GETDATE() AS DATETIME), '2027-07-09T14:52:00', 900, 144.99),
('9f3ac70d-8fca-4a6b-9f30-a3cf5e53fe7b', CAST (GETDATE() AS DATETIME), '2026-07-16T14:52:00', 900, 76.49),
('2f3ac70d-8fca-4a6b-9f30-a3cf5e53fe7b', CAST (GETDATE() AS DATETIME), '2026-07-16T14:53:00', 700, 66.49),
('8cf0ca06-a320-46d8-b528-1e13799d8035', CAST (GETDATE() AS DATETIME), '2026-07-16T14:53:00', 750, 500.01),
('9f3ac70d-8fca-4a6b-9f30-a3cf5e53fe7b', CAST (GETDATE() AS DATETIME), '2026-07-17T14:52:00', 900, 76.49),
('2f3ac70d-8fca-4a6b-9f30-a3cf5e53fe7b', CAST (GETDATE() AS DATETIME), '2026-07-16T14:53:00', 750, 96.49),
('8cf0ca06-a320-46d8-b528-1e13799d8035', CAST (GETDATE() AS DATETIME), '2026-07-16T14:53:00', 750, 500.01);

SELECT pr.ProductName
FROM   Purchases AS pc
       INNER JOIN
       Products AS pr
       ON pc.ProductID = pr.ProductID;

SELECT   pr.ProductName AS [Название],
         COUNT(*) AS [Количество] -- Выборка с группировкой 
FROM     Purchases AS pc
         LEFT OUTER JOIN
         Products AS pr
         ON pc.ProductID = pr.ProductID
GROUP BY ProductName;

INSERT  INTO Categories (CategoryID, CategoryName)
VALUES                 ('4bf571bf-56e4-4e18-83f2-2697352b01c5', 'Сладкое');

SELECT c.CategoryName AS [Категория],
       p.ProductName AS [Продукт] -- Левое соединение
FROM   Categories AS c
       LEFT OUTER JOIN
       Products AS p
       ON c.CategoryID = P.CategoryID;

SELECT c.CategoryName AS [Категория],
       p.ProductName AS [Продукт] -- Правое соединение
FROM   Products AS p
       RIGHT OUTER JOIN
       Categories AS c
       ON p.CategoryID = c.CategoryID;

SELECT c.CategoryName AS [Категория],
       p.ProductName AS [Продукт] -- Внутреннее соединение
FROM   Products AS p
       INNER JOIN
       Categories AS c
       ON p.CategoryID = c.CategoryID;