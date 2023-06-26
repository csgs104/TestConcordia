namespace ConcordiaDBInitializer;

using Microsoft.Data.SqlClient;

public static class Initializer
{
    // DATABASE (Board in Trello)
    private const string n0 = "Concordia";
    private const string db = @$"
    IF NOT EXISTS (SELECT * FROM [sys].[databases] WHERE [name]='{n0}') 
    BEGIN CREATE DATABASE [{n0}] END";

    // SCIENTISTS (Members in Trello)
    private const string n1 = "Scientists";
    private const string t1 = @$"
    IF NOT EXISTS (SELECT * FROM [{n0}].[sys].[sysobjects] WHERE [name]='{n1}' and [xtype]='U') 
    BEGIN 
    CREATE TABLE [{n0}].[dbo].[{n1}] (
    [Id] 	       INT IDENTITY(1,1) NOT NULL,
    [Code]         VARCHAR(24) NULL,
    [FullName]     VARCHAR(100) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ) END";

    // STATES (Lists in Trello) of the experiments
    private const string n2 = "States";
    private const string t2 = @$"
    IF NOT EXISTS (SELECT * FROM [{n0}].[sys].[sysobjects] WHERE [name]='{n2}' and [xtype]='U') 
    BEGIN 
    CREATE TABLE [{n0}].[dbo].[{n2}] (
    [Id] 	       INT IDENTITY(1,1) NOT NULL,
    [Code] 	       VARCHAR(24) NULL,
    [Name] 	       VARCHAR(100) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ) END";

    // PRIORITIES (Labels in Trello) of the experiments
    private const string n3 = "Priorities";
    private const string t3 = @$"
    IF NOT EXISTS (SELECT * FROM [{n0}].[sys].[sysobjects] WHERE [name]='{n3}' and [xtype]='U') 
    BEGIN 
    CREATE TABLE [{n0}].[dbo].[{n3}] (
    [Id] 	       INT IDENTITY(1,1) NOT NULL,
    [Code] 	       VARCHAR(24) NULL,
    [Name] 	       VARCHAR(100) NOT NULL,
    [Color] 	   VARCHAR(100) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ) END";

    // EXPERIMENTS (Cards in Trello)
    private const string n4 = "Experiments";
    private const string t4 = @$"
    IF NOT EXISTS (SELECT * FROM [{n0}].[sys].[sysobjects] WHERE [name]='{n4}' and [xtype]='U') 
    BEGIN 
    CREATE TABLE [{n0}].[dbo].[{n4}] (
    [Id] 	       INT IDENTITY(1,1) NOT NULL,
    [Code] 	       VARCHAR(24) NULL,
    [Name] 	       VARCHAR(100) NOT NULL,
    [Description]  VARCHAR(MAX) NOT NULL,
    [Loaded]  	   BIT NOT NULL,
    [StartDate]    DATETIME NULL,
    [DueDate] 	   DATETIME NULL,
    [PriorityId]   INT NOT NULL,
    [StateId] 	   INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([PriorityId])   REFERENCES [dbo].[{n3}] ([Id]),
    FOREIGN KEY ([StateId])      REFERENCES [dbo].[{n2}] ([Id])
    ) END";

    // Remarks (Comments on Trello) written on a experiment by a scientist
    private const string n5 = "Remarks";
    private const string t5 = @$"
    IF NOT EXISTS (SELECT * FROM [{n0}].[sys].[sysobjects] WHERE [name]='{n5}' and [xtype]='U') 
    BEGIN 
    CREATE TABLE [{n0}].[dbo].[{n5}] (
    [Id] 	       INT IDENTITY(1,1) NOT NULL,
    [Code]   	   VARCHAR(24) NULL,
    [Text] 	       VARCHAR(MAX) NOT NULL,
    [Date] 	       DATETIME NULL,
    [ScientistId]  INT NOT NULL,
    [ExperimentId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ExperimentId]) REFERENCES [dbo].[{n4}] ([Id]),
    FOREIGN KEY ([ScientistId])  REFERENCES [dbo].[{n1}] ([Id])
    ) END";

    // members that PARTECIPATE on a task
    private const string n6 = "Participants";
    private const string t6 = @$"
    IF NOT EXISTS (SELECT * FROM [{n0}].[sys].[sysobjects] WHERE [name]='{n6}' and [xtype]='U') 
    BEGIN 
    CREATE TABLE [{n0}].[dbo].[{n6}] (
    [Id] 	       INT IDENTITY(1,1) NOT NULL,
    [ScientistId]  INT NULL,
    [ExperimentId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ExperimentId]) REFERENCES [dbo].[{n4}] ([Id]),
    FOREIGN KEY ([ScientistId])  REFERENCES [dbo].[{n1}] ([Id])
    ) END";

    private const string i1 = @$"
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n1}] WHERE [Id] = 1) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n1}] 	     VALUES (NULL,'Marco Viola') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n1}] WHERE [Id] = 2) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n1}] 	     VALUES (NULL,'Mario Rossi') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n1}] WHERE [Id] = 3) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n1}] 	     VALUES (NULL,'Piero Verdi') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n1}] WHERE [Id] = 4) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n1}] 	     VALUES (NULL,'Carlo Fiore') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n1}] WHERE [Id] = 5) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n1}] 	     VALUES (NULL,'James Rossi') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n1}] WHERE [Id] = 6) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n1}] 	     VALUES (NULL,'Rocco Gallo') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n1}] WHERE [Id] = 7) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n1}] 	     VALUES (NULL,'Daisy Rossi') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n1}] WHERE [Id] = 8) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n1}] 	     VALUES (NULL,'Grace Verdi') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n1}] WHERE [Id] = 9) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n1}] 	     VALUES (NULL,'Lucia Fiume') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n1}] WHERE [Id] = 10) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n1}] 	     VALUES (NULL,'Jenny Rossi') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n1}] WHERE [Id] = 11) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n1}] 	     VALUES (NULL,'Diana Gallo') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n1}] WHERE [Id] = 12) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n1}] 	     VALUES (NULL,'Pippo Rossi') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n1}] WHERE [Id] = 13) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n1}] 	     VALUES (NULL,'Betty Fiume') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n1}] WHERE [Id] = 14) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n1}] 	     VALUES (NULL,'Alice Fiore') END";

    private const string i2 = @$"
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n2}] WHERE [Id] = 1) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n2}] 	     VALUES (NULL,'Start') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n2}] WHERE [Id] = 2) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n2}] 	     VALUES (NULL,'Working') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n2}] WHERE [Id] = 3) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n2}] 	     VALUES (NULL,'Finish') END";

    private const string i3 = @$"
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n3}] WHERE [Id] = 1) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n3}] 	     VALUES (NULL,'HIGH','RED') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n3}] WHERE [Id] = 2) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n3}] 	     VALUES (NULL,'MEDIUM','YELLOW') END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n3}] WHERE [Id] = 3) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n3}] 	     VALUES (NULL,'LOW','GREEN') END";

    private const string i4 = @$"
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n4}] WHERE [Id] = 1) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n4}] 	     VALUES (NULL,'Exp1','Description1',1,'2023/05/20',NULL,3,3) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n4}] WHERE [Id] = 2) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n4}] 	     VALUES (NULL,'Exp2','Description2',1,'2023/05/21',NULL,3,2) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n4}] WHERE [Id] = 3) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n4}] 	     VALUES (NULL,'Exp3','Description3',0,'2023/05/22',NULL,2,2) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n4}] WHERE [Id] = 4) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n4}] 	     VALUES (NULL,'Exp4','Description4',0,'2023/05/23',NULL,1,3) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n4}] WHERE [Id] = 5) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n4}] 	     VALUES (NULL,'Exp5','Description5',1,'2023/05/24',NULL,1,1) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n4}] WHERE [Id] = 6) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n4}] 	     VALUES (NULL,'Exp6','Description6',0,'2023/05/25',NULL,2,1) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n4}] WHERE [Id] = 7) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n4}] 	     VALUES (NULL,'Exp7','Description7',0,'2023/05/26',NULL,1,2) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n4}] WHERE [Id] = 8) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n4}] 	     VALUES (NULL,'Exp8','Description8',1,'2023/05/27',NULL,3,3) END";

    private const string i5 = @$"
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 1) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Remark01','2023/05/20',1,1) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 2) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Remark02','2023/05/21',2,2) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 3) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Remark03','2023/05/22',3,3) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 4) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Comment04','2023/05/23',4,4) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 5) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Remark05','2023/05/24',5,5) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 6) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Remark06','2023/05/25',6,6) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 7) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Remark07','2023/05/26',7,7) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 8) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Remark08','2023/05/27',8,8) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 9) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Remark09','2023/05/21',9,1) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 10) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Remark10','2023/05/22',10,2) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 11) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Remark11','2023/05/23',11,3) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 12) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Remark12','2023/05/24',12,4) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 13) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Remark13','2023/05/25',13,5) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 14) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Remark14','2023/05/26',14,6) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 15) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Remark15','2023/05/27',1,7) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n5}] WHERE [Id] = 16) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n5}] 	     VALUES (NULL,'Remark16','2023/05/28',2,8) END";

    private const string i6 = @$"
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 1) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (1,1) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 2) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (2,2) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 3) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (3,3) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 4) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (4,4) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 5) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (5,5) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 6) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (6,6) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 7) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (7,7) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 8) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (8,8) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 9) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (9,1) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 10) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (10,2) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 11) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (11,3) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 12) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (12,4) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 13) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (13,5) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 14) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (14,6) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 15) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (1,7) END
    IF NOT EXISTS (SELECT * FROM [{n0}].[dbo].[{n6}] WHERE [Id] = 16) 
    BEGIN INSERT INTO [{n0}].[dbo].[{n6}] 	     VALUES (2,8) END";

    private const string d0 = @$"
    IF EXISTS (SELECT * FROM [sys].[databases] WHERE [name]='{n0}') 
    BEGIN DROP DATABASE [{n0}] END";

    public static void Create(string connectionstring)
    {
        using var cn = new SqlConnection(connectionstring);
        using var cmdD0 = new SqlCommand(db, cn);
        using var cmdT1 = new SqlCommand(t1, cn);
        using var cmdT2 = new SqlCommand(t2, cn);
        using var cmdT3 = new SqlCommand(t3, cn);
        using var cmdT4 = new SqlCommand(t4, cn);
        using var cmdT5 = new SqlCommand(t5, cn);
        using var cmdT6 = new SqlCommand(t6, cn);
        try
        {
            Console.WriteLine("START.");
            Console.WriteLine("...");

            Console.Write("OPEN CONNECTION...\t\t");
            cn.Open();
            Console.WriteLine("OK!");

            Console.Write("CREATE DATABASE...\t\t");
            cmdD0.ExecuteNonQuery();
            Console.WriteLine("OK!");

            Console.Write("CREATE TABLE T1...\t\t");
            cmdT1.ExecuteNonQuery();
            Console.WriteLine("OK!");

            Console.Write("CREATE TABLE T2...\t\t");
            cmdT2.ExecuteNonQuery();
            Console.WriteLine("OK!");

            Console.Write("CREATE TABLE T3...\t\t");
            cmdT3.ExecuteNonQuery();
            Console.WriteLine("OK!");

            Console.Write("CREATE TABLE T4...\t\t");
            cmdT4.ExecuteNonQuery();
            Console.WriteLine("OK!");

            Console.Write("CREATE TABLE T5...\t\t");
            cmdT5.ExecuteNonQuery();
            Console.WriteLine("OK!");

            Console.Write("CREATE TABLE T6...\t\t");
            cmdT6.ExecuteNonQuery();
            Console.WriteLine("OK!");

            Console.WriteLine("...");
            Console.WriteLine("FINISH.");
        }
        catch (SqlException ex)
        {
            Console.Error.WriteLine(ex);
        }
    }

    public static void Insert(string connectionstring)
    {
        using var cn = new SqlConnection(connectionstring);
        using var cmdI1 = new SqlCommand(i1, cn);
        using var cmdI2 = new SqlCommand(i2, cn);
        using var cmdI3 = new SqlCommand(i3, cn);
        using var cmdI4 = new SqlCommand(i4, cn);
        using var cmdI5 = new SqlCommand(i5, cn);
        using var cmdI6 = new SqlCommand(i6, cn);
        try
        {
            Console.WriteLine("START.");
            Console.WriteLine("...");

            Console.Write("OPEN CONNECTION...\t\t");
            cn.Open();
            Console.WriteLine("OK!");

            Console.Write("INSERT INTO TABLE T1...\t\t");
            cmdI1.ExecuteNonQuery();
            Console.WriteLine("OK!");

            Console.Write("INSERT INTO TABLE T2...\t\t");
            cmdI2.ExecuteNonQuery();
            Console.WriteLine("OK!");

            Console.Write("INSERT INTO TABLE T3...\t\t");
            cmdI3.ExecuteNonQuery();
            Console.WriteLine("OK!");

            Console.Write("INSERT INTO TABLE T4...\t\t");
            cmdI4.ExecuteNonQuery();
            Console.WriteLine("OK!");

            Console.Write("INSERT INTO TABLE T5...\t\t");
            cmdI5.ExecuteNonQuery();
            Console.WriteLine("OK!");

            Console.Write("INSERT INTO TABLE T6...\t\t");
            cmdI6.ExecuteNonQuery();
            Console.WriteLine("OK!");

            Console.WriteLine("...");
            Console.WriteLine("FINISH.");
        }
        catch (SqlException ex)
        {
            Console.Error.WriteLine(ex);
        }
    }

    public static void Delete(string connectionstring)
    {
        using var cn = new SqlConnection(connectionstring);
        using var cmdD0 = new SqlCommand(d0, cn);
        try
        {
            Console.WriteLine("START.");
            Console.WriteLine("...");

            Console.Write("OPEN CONNECTION...\t\t");
            cn.Open();
            Console.WriteLine("OK!");

            Console.Write("DELETE DATABASE...\t\t");
            cmdD0.ExecuteNonQuery();
            Console.WriteLine("OK!");

            Console.WriteLine("...");
            Console.WriteLine("FINISH.");
        }
        catch (SqlException ex)
        {
            Console.Error.WriteLine(ex);
        }
    }

    public static void Query()
    {
        var query = $@"
        SELECT ??.[], ??.[] AS ..., ??.[] AS ...
        FROM [{n0}].[dbo].[] ??
        JOIN [{n0}].[dbo].[] ?? ON ??.[] = ??.[]
        JOIN [{n0}].[dbo].[] ?? ON ??.[] = ??.[]
        JOIN [{n0}].[dbo].[] cr ON ??.[] = ??.[]
        WHERE ??.[] = '!!'";

        Console.WriteLine("Query1: " + query);

        Console.WriteLine("...");

        Console.WriteLine("END");
    }

}