![NuGet Version](https://img.shields.io/nuget/vpre/cewno.HPACKTool?style=flat)
![GitHub Release](https://img.shields.io/github/v/release/cewno/HPACKTool)
![NuGet Downloads](https://img.shields.io/nuget/dt/cewno.HPACKTool?label=nuget%20downloads)

English is translated by AI!  
英语由 AI 翻译！

This tool is used to decode the HTTP2 HPACK compression algorithm specified in [RFC7541](https://www.rfc-editor.org/rfc/rfc7541)  
这个工具用于解码HTTP2所用的HPACK压缩算法，HPACK压缩算法由 [RFC7541](https://www.rfc-editor.org/rfc/rfc7541) 制定  

This tool does not include decoder and encoder instances and utility classes for decoding and encoding different types 
of header fields, but only utility classes for decoding and encoding strings and integers  
这个工具不包括解码器实例和编码器实例以及解码和编码不同类型的头字段的工具类，仅包括解码和编码字符串及整数的工具类

If you find a Bug, feel free to file it as an [issue](https://github.com/cewno/HPACKTool/issues)  
如果你发现Bug，请大胆的通过[issue](https://github.com/cewno/HPACKTool/issues)告诉我  
# install 安装
 dotnet:
```shell
dotnet add package cewno.HPACKTool
```
Package Manager:
```shell
NuGet\Install-Package cewno.HPACKTool
```
PackageReference:
```xml
<PackageReference Include="cewno.HPACKTool" Version="3.0.0" />
```
