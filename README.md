<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/547236804/22.1.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1122248)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# File Manager for DevExtreme - How to implement a custom provider

The sample illustrates how to implement your own [custom provider](https://js.devexpress.com/Documentation/ApiReference/UI_Components/dxFileManager/File_System_Providers/Custom/) for DevExtreme. In the sample, the file manager is placed on the Razor page, and ajax requests are sent to the server to interact with folders/files on the server.

Server-side actions return data in the following format:

```xml
{
  "success": true,
  "errorCode": null,
  "errorText": "",
  "result": [...]
}
```

Based on this data, you can throw an error using the [DevExpress.fileManagement.FileSystemError](https://js.devexpress.com/Documentation/ApiReference/Common/Object_Structures/FileSystemError/) class or return your result. For example, for the renameItem method, the following code is used:

```js
renameItem: function (item, newName) {
    var url = "/api/data/RenameItem";
    var data = { pathInfo: item.key, newName: newName };
    return SendAjaxRequest(url, data);
},

function SendAjaxRequest(url, data, requestType = "get") {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: url,
            data: data,
            type: requestType,
            dataType: 'json',
            contentType: requestType == "get" ? "application/json; charset=utf-8" : false,
            processData: requestType == "get",
            async: false,
            success: function (response) {
                if (response.success == false) {
                    reject(new DevExpress.fileManagement.FileSystemError(response.errorCode, null, response.errorText));
                }
                else {
                    resolve(response.result)
                }
            }
        });
    });
}
```

```cs
        [Route("RenameItem")]
        [HttpGet]
        public string RenameItem(string pathInfo, string newName) {
            string response = string.Empty;
            FileAttributes attr = System.IO.File.GetAttributes(pathInfo);
            if (attr.HasFlag(FileAttributes.Directory)) {
                DirectoryInfo di = new DirectoryInfo(pathInfo);
                string folderName = Path.GetDirectoryName(pathInfo);
                string newDirectory = Path.Combine(di.Parent.FullName, newName);
                if (Directory.Exists(newDirectory)) {
                    return this.CreateResponse(false, 409, "The target directory already exists", null);
                }
                di.MoveTo(newDirectory);
            } else {
                FileInfo fi = new FileInfo(pathInfo);
                string newpath = Path.Combine(fi.Directory.FullName, newName);                
                if (fi.Exists) {
                    return this.CreateResponse(false, 409, "The target file already exists", null);
                }
                fi.MoveTo(newpath);
            }
            return this.CreateResponse(true, null, null, null); ;
        }
        string CreateResponse(bool success, int? errorCode, string errorText, FileDataItem[] result) {
            var serializerSettings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };            
            return JsonConvert.SerializeObject(new { success, errorCode, errorText, result }, serializerSettings);
        }
```


## Files to Review
- [Index.cshtml](./CS/Pages/Index.cshtml)
- [DataController.cs](./CS/Controllers/DataController.cs)
