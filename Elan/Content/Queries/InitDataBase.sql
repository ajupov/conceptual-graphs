use [master];

if(not exists (
	select
		[name]
		from master.dbo.sysdatabases
		where [name] = 'elan')) begin
	create database elan;
end;