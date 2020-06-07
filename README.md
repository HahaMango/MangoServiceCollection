# MangoServiceCollection

[![Build Status](https://dev.azure.com/q932104843/MangoServiceCollection/_apis/build/status/HahaMango.MangoServiceCollection?branchName=master)](https://dev.azure.com/q932104843/MangoServiceCollection/_build/latest?definitionId=6&branchName=master)

## 结构介绍

每个项目均由一个`Asp.net core`项目和一个`类库`项目组成，类库项目中`Modles`文件夹存放数据库实体，Dto对象等，`Repositories`文件夹存放项目相关实体的仓储接口，`Services`文件夹存放服务接口。

由每个项目的`Asp.net core`项目引用类库项目。

> `类库`项目只包含项目接口。具体实现全部在`Asp.net core`项目中

## Mango.Service.Blog

博客接口，包含博客所有的操作。

博客接口项目中的用户Id跟`UserCenter`中的用户Id一致，表示同一个用户。而博客项目中的`User`表为该项目特有的用户信息属性字段。

## Mango.Service.UserCenter

用户中心，总鉴权服务。采用JWT令牌进行认证授权，以后再考虑添加`OAuth2.0`。

> 目前项目采用完全信任前端的方式来管理jwt令牌，即不对令牌作失效，白名单处理，只有过期自动失效的方式。当用户修改密码，登出时由前端主动丢弃令牌。主要原因希望不引入数据库或redis缓存来维持jwt无状态的特性。以后看实际效果考虑是否后台管理令牌。

提供一个总的用户中心，公共的用户属性（每个项目也可以有每个项目特定的额外用户属性，并存放于每个项目的`类库`项目中作为额外的表结构）。

## Mango.Service.ConfigCenter

提供全局配置和单独模块配置，IP白名单。拦截在IP白名单外的地址访问。
