# cert-checker

## Build

```shell
$ dotnet build
```

## Run

```shell
$ bin/Debug/net6.0/CertChecker https://google.com https://baidu.com
Wait a moment...
02/21/2022 CN=*.google.com
02/21/2022 CN=www.google.com
02/25/2022 CN=www.baidu.cn, O="BeiJing Baidu Netcom Science Technology Co., Ltd", S=北京市, C=CN
Done!
```
