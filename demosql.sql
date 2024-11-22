CREATE DATABASE ZestyBite;
USE ZestyBite;

-- 1. Role Table
CREATE TABLE `Role` (
    Role_ID TINYINT NOT NULL PRIMARY KEY,
    Role_Description ENUM('Manager', 'Order Taker',
    'Procurement Manager','Server Staff',
    'Customer Service Staff', 'Food Runner', 'Customer') NOT NULL
);

-- 2. Account Table
CREATE TABLE `Account` (
    Account_ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    Username VARCHAR(255) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Name VARCHAR(255) NOT NULL,
    Phone_Number VARCHAR(20) NOT NULL,
    Address VARCHAR(255) NOT NULL,
    Gender INT NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Verification_Code VARCHAR(50) NOT NULL,
    Profile_Image TEXT,
    Role_ID TINYINT NOT NULL,
    AccessFailedCount INT NOT NULL DEFAULT 0,
    ConcurrencyStamp VARCHAR(256) NULL,
    EmailConfirmed BIT NOT NULL DEFAULT 0,
    PhoneNumberConfirmed BIT NOT NULL DEFAULT 0,
    TwoFactorEnabled BIT NOT NULL DEFAULT 0,
    LockoutEnabled BIT NOT NULL DEFAULT 0,
    LockoutEnd DATETIME NULL,
    NormalizedEmail VARCHAR(256) NULL,
    NormalizedUserName VARCHAR(256) NULL,
    SecurityStamp VARCHAR(256) NULL,
    FOREIGN KEY (Role_ID) REFERENCES Role(Role_ID)
);

-- 3. Payment Table
CREATE TABLE Payment (
    Payment_ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    Payment_Method INT NOT NULL
);

-- 4. Item Table
CREATE TABLE Item (
    Item_ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    Item_Name VARCHAR(255) NOT NULL,
    Item_Category ENUM('Main course',
    'Dessert', 'Drink', 'Salad', 'Fruit') NOT NULL,
    Item_Status INT NOT NULL,
    Item_Description VARCHAR(255) NOT NULL,
    Suggested_Price DECIMAL(12, 0) NOT NULL,
    Item_Image TEXT NOT NULL,
    Is_Served INT NOT NULL
);

-- 5. Table Table
CREATE TABLE `Table` (
    Table_ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    Table_Capacity INT NOT NULL,
    Table_Maintenance INT NOT NULL,
    Reservation_ID INT,
    Item_ID INT NOT NULL,
    Table_Type INT NOT NULL,
    Table_Status ENUM('Served', 'Empty', 'Waiting', 'Deposit') NOT NULL,
    Table_Note VARCHAR(255),
    Account_ID INT NOT NULL,
    FOREIGN KEY (Item_ID) REFERENCES Item(Item_ID),
    FOREIGN KEY (Account_ID) REFERENCES `Account`(Account_ID)
);

-- 6. Reservation Table
CREATE TABLE Reservation (
    Reservation_ID INT PRIMARY KEY AUTO_INCREMENT,
    Bill_ID INT NOT NULL,
    Table_ID INT NOT NULL,
    Reservation_Start DATETIME NOT NULL,
    Reservation_End DATETIME NOT NULL,
    Reservation_Cost DECIMAL(12, 0) NOT NULL DEFAULT 0,
    FOREIGN KEY (Table_ID) REFERENCES `Table`(Table_ID)
);

-- 7. Bill Table
CREATE TABLE Bill (
    Bill_ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    Bill_Status INT NOT NULL,
    Payment_ID INT NOT NULL,
    Account_ID INT NOT NULL,
    Table_ID INT NOT NULL,
    Total_Cost DECIMAL(12, 0) NOT NULL,
    Bill_Datetime DATETIME NOT NULL,
    Bill_Type INT NOT NULL,
    FOREIGN KEY (Payment_ID) REFERENCES Payment(Payment_ID),
    FOREIGN KEY (Account_ID) REFERENCES `Account`(Account_ID),
    FOREIGN KEY (Table_ID) REFERENCES `Table`(Table_ID)
);

-- 8. Supply Table
CREATE TABLE Supply (
    Supply_ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    Product_Name VARCHAR(255) NOT NULL,
    Supply_Quantity DECIMAL(10, 2) NOT NULL,
    Supply_Price DECIMAL(12, 0) NOT NULL,
    Supply_Status INT NOT NULL,
    Date_Import DATETIME NOT NULL,
    Date_Expiration DATETIME NOT NULL,
    Table_ID INT NOT NULL,
    Item_ID INT NOT NULL,
    Vendor_Name VARCHAR(255) NOT NULL,
    Vendor_Phone VARCHAR(255) NOT NULL UNIQUE,
    Vendor_Address VARCHAR(255) NOT NULL,
    Supply_Category ENUM('Food', 'Drink', 'Facility') NOT NULL,
    FOREIGN KEY (Table_ID) REFERENCES `Table`(Table_ID),
    FOREIGN KEY (Item_ID) REFERENCES Item(Item_ID)
);

-- 9. Feedback Table
CREATE TABLE Feedback (
    Fb_ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    Fb_Content VARCHAR(255) NOT NULL,
    Fb_Datetime DATETIME NOT NULL,
    Account_ID INT NOT NULL,
    Item_ID INT NOT NULL,
    FOREIGN KEY (Account_ID) REFERENCES `Account`(Account_ID),
    FOREIGN KEY (Item_ID) REFERENCES Item(Item_ID)
);

-- 10. Profit Table
CREATE TABLE Profit (
    Date DATE NOT NULL PRIMARY KEY,
    Supply_ID INT NOT NULL,
    Bill_ID INT NOT NULL,
    Profit_Ammount DECIMAL NOT NULL,
    FOREIGN KEY (Supply_ID) REFERENCES Supply(Supply_ID),
    FOREIGN KEY (Bill_ID) REFERENCES Bill(Bill_ID)
);

-- 11. Table Details Table
CREATE TABLE Table_Details (
    Table_ID INT NOT NULL,
    Item_ID INT NOT NULL,
    PRIMARY KEY (Table_ID, Item_ID),
    Quantity INT NOT NULL,
    FOREIGN KEY (Table_ID) REFERENCES `Table`(Table_ID),
    FOREIGN KEY (Item_ID) REFERENCES Item(Item_ID)
);

-- 12. Supply Item Table
CREATE TABLE Supply_Item (
    Supply_ID INT NOT NULL,
    Item_ID INT NOT NULL,
    PRIMARY KEY (Supply_ID, Item_ID),
    FOREIGN KEY (Supply_ID) REFERENCES Supply(Supply_ID),
    FOREIGN KEY (Item_ID) REFERENCES Item(Item_ID)
);

ALTER TABLE `Table`
    ADD FOREIGN KEY (Reservation_ID) REFERENCES Reservation(Reservation_ID);
    
ALTER TABLE Reservation
    ADD FOREIGN KEY (Bill_ID) REFERENCES Bill(Bill_ID);
    
INSERT INTO `Role` (Role_ID, Role_Description)
    VALUES (1, 'Manager'), (2, 'Order Taker'), (3, 'Procurement Manager'),
    (4, 'Server Staff'), (5, 'Customer Service Staff'), (6, 'Food Runner'), (7, 'Customer');
    
-- SELECT * FROM `Role`

-- --------------------------------------------------------------------------------------------------	------------------------------------------------------------------------------------------------------------------------------

INSERT INTO `Account` (Username, Password, Name, Phone_Number, Address, Gender, Email, Verification_Code, Profile_Image, Role_ID, AccessFailedCount, ConcurrencyStamp, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled)
VALUES 
    ('john_doe', 'password123', 'John Doe', '123-456-7890', '123 Elm St, Springfield', 1, 'john@example.com', 'verifcode1', NULL, 6, 0, NULL, 0, 0, 0, 0),
    ('jane_smith', 'password456', 'Jane Smith', '098-765-4321', '456 Oak St, Springfield', 0, 'jane@example.com', 'verifcode2', NULL, 5, 0, NULL, 0, 0, 0, 0),
    ('admin_user', 'adminpass', 'Admin User', '555-555-5555', '789 Pine St, Springfield', 1, 'admin@example.com', 'verifcode3', NULL, 1, 0, NULL, 0, 0, 0, 0),
    ('order_taker1', 'ordertakerpass', 'Order Taker', '111-222-3333', '321 Maple St, Springfield', 1, 'ordertaker@example.com', 'verifcode4', NULL, 2, 0, NULL, 0, 0, 0, 0),
    ('procurement_mgr', 'procurementpass', 'Procurement Manager', '444-555-6666', '654 Cedar St, Springfield', 0, 'procurement@example.com', 'verifcode5', NULL, 3, 0, NULL, 0, 0, 0, 0),
    ('server_staff1', 'serverpass', 'Server Staff', '777-888-9999', '987 Birch St, Springfield', 1, 'server@example.com', 'verifcode6', NULL, 4, 0, NULL, 0, 0, 0, 0),
    ('customer1', 'customerpass', 'Customer One', '222-333-4444', '147 Walnut St, Springfield', 1, 'customer1@example.com', 'verifcode7', NULL, 7, 0, NULL, 0, 0, 0, 0);

-- Insert payment methods
INSERT INTO Payment (Payment_ID, Payment_Method)
VALUES 
    (1, 1), 
    (2, 0),
    (3, 1),
    (4, 1),
    (5, 0),
    (6, 1),
    (7, 0),
    (8, 1),
    (9, 0),
    (10, 0),
    (11, 0),
    (12, 1),
    (13, 0),
    (14, 0),
    (15, 1),
    (16, 0),
    (17, 1);

-- Insert items
INSERT INTO Item (Item_Name, Item_Category, Item_Status, Item_Description, Suggested_Price, Item_Image, Is_Served)
VALUES
('Pizza Margherita', 'Main course', 1, 'Classic Italian pizza with tomatoes, mozzarella cheese, and fresh basil.', 120000, '../Content/images/bg_2.png', 1),
('Spaghetti Carbonara', 'Main course', 1, 'Traditional Roman pasta dish made with eggs, cheese, pancetta, and pepper.', 140000, '../Content/images/drink-6.jpg', 1),
('Tiramisu', 'Dessert', 1, 'Delicious coffee-flavored Italian dessert made with mascarpone cheese and cocoa.', 60000, '../Content/images/burger-1.jpg', 1),
('Caesar Salad', 'Salad', 0, 'Crisp romaine lettuce with Caesar dressing, croutons, and parmesan cheese.', 80000, '../Content/images/drink-7.jpg', 0),
('Margarita', 'Drink', 1, 'Classic cocktail made with tequila, lime juice, and triple sec.', 100000, '../Content/images/drink-1.jpg', 1),
('Chocolate Cake', 'Dessert', 0, 'Rich and moist chocolate cake topped with creamy chocolate frosting.', 50000, '../Content/images/drink-9.jpg', 0),
('Fruit Salad', 'Fruit', 1, 'A refreshing mix of seasonal fruits, served chilled.', 70000, '../Content/images/drink-1.jpg', 1),
('Grilled Chicken', 'Main course', 1, 'Juicy grilled chicken breast served with a side of vegetables.', 150000, '../Content/images/burger-3.jpg', 1),
('Lemonade', 'Drink', 0, 'Refreshing homemade lemonade, perfect for a hot day.', 30000, '../Content/images/drink-2.jpg', 0),
('Lasagna', 'Main course', 1, 'Layers of pasta, meat, cheese, and rich tomato sauce baked to perfection.', 130000, '../Content/images/burger-3.jpg', 1),
('Cheesecake', 'Dessert', 1, 'Creamy cheesecake with a graham cracker crust and fruit topping.', 70000, '../Content/images/drink-5.jpg', 1),
('Greek Salad', 'Salad', 0, 'A fresh mix of tomatoes, cucumbers, olives, and feta cheese.', 90000, '../Content/images/drink-4.jpg', 0),
('Iced Tea', 'Drink', 1, 'Chilled tea served with lemon and mint for a refreshing drink.', 20000, '../Content/images/drink-3.jpg', 1),
('Pavlova', 'Dessert', 0, 'Light meringue dessert topped with whipped cream and fresh fruits.', 80000, '../Content/images/drink-7.jpg', 0),
('Fruit Smoothie', 'Fruit', 1, 'A blend of fresh fruits and yogurt, perfect for a healthy snack.', 50000, '../Content/images/drink-9.jpg', 1),
('Beef Stroganoff', 'Main course', 1, 'Tender beef strips cooked in a creamy mushroom sauce served over egg noodles.', 160000, '../Content/images/burger-1.jpg', 1),
('Soda', 'Drink', 0, 'Chilled soft drink available in various flavors.', 10000, '../Content/images/drink-4.jpg', 0);

-- Insert tables
INSERT INTO `Table` (Table_Capacity, Table_Maintenance, Reservation_ID, Item_ID, Table_Type, Table_Status, Table_Note, Account_ID)
VALUES 
    (4, 0, NULL, 1, 0, 'Empty', NULL, 1),
    (2, 0, NULL, 2, 0, 'Empty', NULL, 2);

-- Insert bills
INSERT INTO Bill (Bill_Status, Payment_ID, Account_ID, Table_ID, Total_Cost, Bill_Datetime, Bill_Type)
VALUES 
    (1, 1, 1, 1, 230000, '2024-11-11 20:00:00', 0),
    (2, 1, 2, 2, 150000, '2024-11-11 21:00:00', 1);

-- Insert reservations
INSERT INTO Reservation (Bill_ID, Table_ID, Reservation_Start, Reservation_End, Reservation_Cost)
VALUES 
    (1, 1, '2024-11-11 18:00:00', '2024-11-11 20:00:00', 0),
    (2, 2, '2024-11-11 19:00:00', '2024-11-11 21:00:00', 0);

UPDATE `Table`
    SET Reservation_ID = 1
    WHERE Table_ID = 1;

UPDATE `Table`
    SET Reservation_ID = 2
    WHERE Table_ID = 2;

-- Insert supplies
INSERT INTO Supply (Product_Name, Supply_Quantity, Supply_Price, Supply_Status, Date_Import, Date_Expiration, Table_ID, Item_ID, Vendor_Name, Vendor_Phone, Vendor_Address, Supply_Category)
VALUES 
    ('Chicken', 100, 50000, 1, '2024-11-01', '2025-11-01', 1, 1, 'Vendor A', '0123456789', 'Vendor Address A', 'Food'),
    ('Lettuce', 200, 20000, 1, '2024-11-02', '2025-11-02', 1, 2, 'Vendor B', '0987654321', 'Vendor Address B', 'Food');

-- Insert feedback
INSERT INTO Feedback (Fb_Content, Fb_Datetime, Account_ID, Item_ID)
VALUES
    ('Great food and excellent service!', '2024-10-15 14:30:00', 1, 1),
    ('The pizza was delicious, but the wait was too long.', '2024-10-12 18:45:00', 2, 1),
    ('Loved the spaghetti carbonara!', '2024-10-10 20:00:00', 3, 2),
    ('The cheesecake was divine!', '2024-10-05 19:00:00', 4, 11),
    ('Iced tea was refreshing on a hot day.', '2024-10-01 15:15:00', 5, 13),
    ('The salad was fresh, but a bit bland.', '2024-09-28 12:30:00', 6, 4),
    ('Margarita was perfectly made!', '2024-09-25 21:00:00', 7, 5),
    ('Service was slow, but worth the wait for the food.', '2024-09-20 17:50:00', 1, 3),
    ('Fruit salad was a great healthy option.', '2024-09-18 14:00:00', 2, 7),
    ('The atmosphere was lovely, will definitely return!', '2024-09-15 16:30:00', 3, 6);

-- Insert profit
INSERT INTO Profit (Date, Supply_ID, Bill_ID, Profit_Ammount)
VALUES 
    ('2024-11-10', 1, 1, 50000),
    ('2024-11-11', 2, 2, 30000);

-- Insert table details
INSERT INTO Table_Details (Table_ID, Item_ID, Quantity)
VALUES 
    (1, 1, 2),
    (2, 2, 1);
    
INSERT INTO Supply_Item (Supply_ID, Item_ID)
VALUES 
    (1, 1),
    (2, 2);

ALTER TABLE `Table` MODIFY COLUMN `Reservation_ID` INT NOT NULL;
