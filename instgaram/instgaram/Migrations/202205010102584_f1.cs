namespace instgaram.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class f1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        commenttext = c.String(),
                        Post_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.posts", t => t.Post_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Post_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        photo = c.String(),
                        PostLike = c.Int(nullable: false),
                        user_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FName = c.String(),
                        LName = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        Mobile = c.String(),
                        Email = c.String(),
                        Photo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.friends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        my_id = c.Int(nullable: false),
                        friend1_id = c.Int(nullable: false),
                        Friend_Id = c.Int(),
                        Me_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Friend_Id)
                .ForeignKey("dbo.Users", t => t.Me_Id)
                .Index(t => t.Friend_Id)
                .Index(t => t.Me_Id);
            
            CreateTable(
                "dbo.friend_request",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        sender1_id = c.Int(nullable: false),
                        receiver1_id = c.Int(nullable: false),
                        receiver_Id = c.Int(),
                        sender_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.receiver_Id)
                .ForeignKey("dbo.Users", t => t.sender_Id)
                .Index(t => t.receiver_Id)
                .Index(t => t.sender_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.friend_request", "sender_Id", "dbo.Users");
            DropForeignKey("dbo.friend_request", "receiver_Id", "dbo.Users");
            DropForeignKey("dbo.friends", "Me_Id", "dbo.Users");
            DropForeignKey("dbo.friends", "Friend_Id", "dbo.Users");
            DropForeignKey("dbo.posts", "user_Id", "dbo.Users");
            DropForeignKey("dbo.comments", "User_Id", "dbo.Users");
            DropForeignKey("dbo.comments", "Post_Id", "dbo.posts");
            DropIndex("dbo.friend_request", new[] { "sender_Id" });
            DropIndex("dbo.friend_request", new[] { "receiver_Id" });
            DropIndex("dbo.friends", new[] { "Me_Id" });
            DropIndex("dbo.friends", new[] { "Friend_Id" });
            DropIndex("dbo.posts", new[] { "user_Id" });
            DropIndex("dbo.comments", new[] { "User_Id" });
            DropIndex("dbo.comments", new[] { "Post_Id" });
            DropTable("dbo.friend_request");
            DropTable("dbo.friends");
            DropTable("dbo.Users");
            DropTable("dbo.posts");
            DropTable("dbo.comments");
        }
    }
}
