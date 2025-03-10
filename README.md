# army-game tr
 
Kullanılan Yazılım Tasarım Desenleri:

Factory Pattern: AgentFactory, farklı birim türlerini soyutlayarak oluşturma sürecini kolaylaştırdı.
Strategy Pattern: Seviye atlama mekanizmasını yönetmek için farklı stratejiler (PlayerLevelUpStrategy, BalancedLevelUpStrategy) kullanıldı.
State Pattern: Oyundaki bölümlerin farklı logic ihtiyacını gidermek için kullanıldı. (GamePlayState-MainMenuState) AI Agentlerin tercih logiclerinde de kullanılabilirdi. 
Observer Pattern: Event tabanlı sistem sayesinde bileşenler arasında bağımsızlık sağlandı (OnLevelUp, OnExperienceChanged, OnArmyDestroyed gibi).
MVC/MVP Pattern: Birim ve binalar için Model-View-Controller yapısı kullanıldı (AgentController, AgentData, AgentView).
Dependency Injection: ArmyController gibi sınıflarda bağımlılıklar dışarıdan enjekte edilerek test edilebilirlik artırıldı.

Object Pooling: IPoolable arayüzü ile nesneler havuzlanarak bellek yönetimi iyileştirildi.
Spatial Partition Pattern: Sahneyi bölerek yalnızca ilgili bölümlerde arama yaparak optimizasyon sağlandı.
Dirty Flag: Hareket etmeyen agentlerin bölgelerini güncellememek için dirty flag pattern kulannıldı.


Optimizasyon Çalışmaları:

Sprite Atlas: Oyundaki görsellerin daha verimli işlenmesi için kullanıldı.
Shader Seçimi: Simple Lit shader kullanarak mobil cihazlarda performansı artırdım.
Shadow: Gerçek zamanlı ışık ve gölgeleri kapatarak, fake gölgelerle görsel kaliteyi korudum.
Mesh Optimizasyonu: Reverse engineering ile alınan assetlerin bazıları yüksek vertex sayısına sahipti. Örneğin, Helmet01 modeli 40K vertex içeriyordu. MantisLODEditor ile bunu 400 vertexe düşürdüm.
Görsel Optimizasyon: Görseller için sıkıştırma ve max size ayarlamaları yapıldı bellek ve dosya boyutu optimizasyonu sağlandı.

# army-game en
