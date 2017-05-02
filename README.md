## Migrations

With migrations on a class library project, you can either setup a startup class in the `Domain` project or specify the startup project as one
that already has a startup class, such as the web project:

	dotnet ef migrations add AddAllowanceTask -s ..\Budget.Web

If you want to go the separate Startup class route, it is described out here: https://github.com/aspnet/EntityFramework/issues/5320#issuecomment-221940692

## Identity Framework

The entity framework implementation of ASP.NET Identity appears to use an `nvarchar(450)` datatype for User IDs,
So related tables should set up a foreign key as:

	[Required, MaxLength(450)]
	public string UserId { get; set; }
