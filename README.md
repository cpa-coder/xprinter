## Fast Receipt

### Certificate commands
Reference to [IBM tutorial](https://www.ibm.com/docs/en/api-connect/2018.x?topic=overview-generating-self-signed-certificate-using-openssl)
1. Create a pem file
```bash
openssl req -newkey rsa:2048 -nodes -keyout key.pem -x509 -days 365 -out certificate.pem
```
2. Create a p12 file
```bash
 openssl pkcs12 -inkey key.pem -in certificate.pem -export -out certificate.p12
```

### Inno Setup SignTool command format
Make sure to set signtool path in Inno Setup
```bash 
signtool sign /f "[certificate path]" /fd SHA256 /p [password] /t http://timestamp.digicert.com $f
```