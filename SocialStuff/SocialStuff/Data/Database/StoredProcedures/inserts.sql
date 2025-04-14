USE BankingDB

INSERT INTO USERS(Username, PhoneNumber)
VALUES('a', '243324'), ('b', '253324')

INSERT INTO MESSAGE_TYPE
VALUES('Text'), ('Image'), ('Request'), ('Transfer')

DELETE FROM MESSAGE_TYPE

INSERT INTO FEED_POSTS(Title, Category, Content)
VALUES('Titlu', 'Categorie', 'Content')

select * 
FROM MESSAGES

SELECT *
FROM MESSAGE_TYPE