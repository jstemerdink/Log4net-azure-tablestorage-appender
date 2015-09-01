workflow Log4netCleanup {
    InlineScript
    {
		$StorageAccountName = "{your account name}"
		$StorageAccountKey = "{your access key}"
    
		$Ctx = new-azurestoragecontext -StorageAccountName $StorageAccountName -StorageAccountKey $StorageAccountKey
		$tabName = "log4net"

		$CleanupTime = [DateTime]::UtcNow.AddDays(-30).ToString("o")

		$table = Get-AzureStorageTable –Name $tabName –Context $Ctx
    
        #Create a table query.
        $query = New-Object Microsoft.WindowsAzure.Storage.Table.TableQuery

        #Define columns to select.
        $list = New-Object System.Collections.Generic.List[string]
        $list.Add("Timestamp")

        $query.FilterString = "Timestamp lt datetime'" + $CleanupTime + "'"
        $table.CloudTable.ExecuteQuery($query) | Out-Null

        #Execute the query.
        $entities = $table.CloudTable.ExecuteQuery($query) | 

        #Foreach object delete it
        ForEach-Object{$table.CloudTable.Execute([Microsoft.WindowsAzure.Storage.Table.TableOperation]::Delete($_))}
    }
}