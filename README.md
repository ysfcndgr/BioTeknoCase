# BioTekno Case

Proje, temiz mimari prensipleri, modüler yapı ve kapsamlı test altyapısıyla öne çıkmaktadır.

## Özellikler

- **Modüler Yapı:** SOLID prensiplerine uygun olarak katmanlı bir mimari kullanılmıştır.
- **RESTful API:** Farklı sistemlerle kolay entegrasyon için RESTful API endpoint’leri mevcuttur.
- **Veritabanı Desteği:** Entity Framework Core kullanılarak MySQL ile uyumlu çalışır.
- **Test Entegrasyonu:** Birim ve entegrasyon testleri sayesinde yüksek güvenilirlik sağlanır.
- **[Diğer Özellikler]:** Onion Arhitecture, MediaTR, CQRS kullanılmıştır.
- **[Diğer Teknolojiler]:** Örneğin, MediatR, AutoMapper, Swagger, Serilog, MySQL, RabbitMQ, Redis

## Kurulum

Projeyi indirin ve WebApi içerisindeki appsettings.json dosyasındaki connection stringleri kendi sisteminizdekilerle değiştirin.
Ardından WebApi projesini çalıştırın.

Program.Cs içerisindeki aşağıdaki kod ile otomatik migration almaktadır.

Swagger ekranını gördüğünüzde
![image](https://github.com/user-attachments/assets/ec03f720-6a53-4f0a-9aff-cc54d3a6b53e)

Bu 2 endpoint ile redis ve rabbitmq'nün çalışıp çalışmadığını test edebilirsiniz.

Orders endpointinin çıktısı.
![image](https://github.com/user-attachments/assets/1abd7835-6f57-448e-82a6-d4b588049423)
![image](https://github.com/user-attachments/assets/49cd8f93-b9c2-4e91-a60d-0ca46242b371)

Case'de mail atma işlemi yoktu ancak bunu'da ekledim.

![image](https://github.com/user-attachments/assets/7de8ae53-73cd-4f3e-a828-9b41dea440ca)

# Products
Products endpointinin çıktısı.

![image](https://github.com/user-attachments/assets/0b18676f-17fe-47a7-9b79-284e6c6094cb)
