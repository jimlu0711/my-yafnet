# Readme.md

這個是從 YAFNET 的 3.2.8 切出來的版本, 因為 YAFNET 己經棄守 .NetFramework 平台, 並改用 .Net Core, 改版至 4.0,
所以我從它的 3.2.8 拉出來之後, 把幾個問題修正之後,讓它可以搭配 MSSQL 開始運作了。

* 原本的 [recommended.web.config] 的 library 設定有誤。 [4.2.1.0] 要改成 [4.2.0.1]

```xml
      <dependentAssembly>
        <assemblyIdentity name="System.Threading.Tasks.Extensions" publicKeyToken="cc7b13ffcd2ddd51"/>
        <bindingRedirect oldVersion="0.0.0.0-4.2.0.1" newVersion="4.2.0.1"/>
      </dependentAssembly>
```


---



**YetAnotherForum.NET** 

原本的說明文件，請至 https://github.com/YAFNET/YAFNET 查看


## Prerequisites:
* ASP.NET .NET Framework 4.8.1 


