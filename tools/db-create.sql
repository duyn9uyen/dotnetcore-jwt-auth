CREATE DATABASE UserDb

GO

USE UserDb

GO

CREATE TABLE LoginModel
  (
     Id                         BIGINT IDENTITY PRIMARY KEY,
     UserName                   NVARCHAR(100) NULL,
     Password                   NVARCHAR(100) NULL,
     RefreshToken               NVARCHAR(max) NULL,
     RefreshTokenExpiryTime     Datetime2 NULL
  )

GO

INSERT INTO LoginModel
VALUES (
    'johndoe',
    'def@123',
    null,
    null
)