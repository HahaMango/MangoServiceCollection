# MangoServiceCollection

[![Build Status](https://dev.azure.com/q932104843/MangoServiceCollection/_apis/build/status/HahaMango.MangoServiceCollection?branchName=master)](https://dev.azure.com/q932104843/MangoServiceCollection/_build/latest?definitionId=6&branchName=master)

## 结构介绍

每个项目均由一个`Asp.net core`项目和一个`类库`项目组成，类库项目中`Modles`文件夹存放数据库实体，Dto对象等，`Repositories`文件夹存放项目相关实体的仓储接口，`Services`文件夹存放服务接口。

由每个项目的`Asp.net core`项目引用类库项目。

> `类库`项目只包含项目接口。具体实现全部在`Asp.net core`项目中

## 架构

![0rs1nx.png](https://s1.ax1x.com/2020/10/09/0rs1nx.png)

上图就是服务的基本部署方案图，目前而言还谈不上什么架构的只是说明一下部署方案而已。

上图中的各个服务器可以看作是一个个docker容器（目前我也的确是用docker部署在同一台机器中，O(∩_∩)O），首先就是Nginx作一个最基本的反向代理，把对应的请求转发到相应的服务（如果以后业务复杂的情况下可能需要使用`Ocelot`等网关）。

然后`consul`即作为配置中心，对于每个服务在启动时从`consul`中获取生产配置和完成在`consul`的服务注册。

剩下的就是每个服务的`mysql`和`redis`的连接了。

## Mango.Service.Blog

博客接口，包含博客所有的操作。

博客接口项目中的用户Id跟`UserCenter`中的用户Id一致，表示同一个用户。而博客项目中的`User`表为该项目特有的用户信息属性字段。

## Mango.Service.UserCenter

用户中心，总鉴权服务。采用JWT令牌进行认证授权，以后再考虑添加`OAuth2.0`。

> 目前项目采用完全信任前端的方式来管理jwt令牌，即不对令牌作失效，白名单处理，只有过期自动失效的方式。当用户修改密码，登出时由前端主动丢弃令牌。主要原因希望不引入数据库或redis缓存来维持jwt无状态的特性。以后看实际效果考虑是否后台管理令牌。

提供一个总的用户中心，公共的用户属性（每个项目也可以有每个项目特定的额外用户属性，并存放于每个项目的`类库`项目中作为额外的表结构）。

## ~~Mango.Service.ConfigCenter~~

> 目前弃用，采用consul作为配置中心。

提供全局配置和单独模块配置，IP白名单。拦截在IP白名单外的地址访问。

## Mango.Service.Image

***计划中，第一版提供功能图片的增删查改，快速缩略图。***
