# **Army Game - TR**

## **Kullanılan Yazılım Tasarım Desenleri**  

### **Creational Patterns**  
- **Factory Pattern:** `AgentFactory`, farklı birim türlerini soyutlayarak oluşturma sürecini kolaylaştırdı.  

### **Behavioral Patterns**  
- **Strategy Pattern:** Seviye atlama mekanizmasını yönetmek için farklı stratejiler (`PlayerLevelUpStrategy`, `BalancedLevelUpStrategy`) kullanıldı.  
- **State Pattern:** Oyundaki bölümlerin farklı mantık ihtiyaçlarını gidermek için kullanıldı. (`GamePlayState`, `MainMenuState`)  
  - **AI Agent**'lerin tercih mantıklarında da kullanılabilirdi.  
- **Observer Pattern:** Event tabanlı sistem sayesinde bileşenler arasında bağımsızlık sağlandı.  
  - Örnek olaylar: `OnLevelUp`, `OnExperienceChanged`, `OnArmyDestroyed`.  

### **Structural Patterns**  
- **MVC/MVP Pattern:** Birim ve binalar için **Model-View-Controller** yapısı kullanıldı.  
  - Örnekler: `AgentController`, `AgentData`, `AgentView`.  
- **Dependency Injection:** `ArmyController` gibi sınıflarda bağımlılıklar dışarıdan enjekte edilerek **test edilebilirlik** artırıldı.  

---

## **Optimizasyon Teknikleri**  

- **Object Pooling:** `IPoolable` arayüzü ile nesneler havuzlanarak bellek yönetimi iyileştirildi.  
- **Spatial Partition Pattern:** Sahneyi bölerek yalnızca ilgili bölümlerde arama yaparak optimizasyon sağlandı.  
- **Dirty Flag Pattern:** Hareket etmeyen agent'lerin bölgelerini güncellememek için kullanıldı.  

---

## **Performans İyileştirmeleri**  

### **Görsel Optimizasyon**  
- **Sprite Atlas:** Oyundaki görsellerin daha verimli işlenmesi için kullanıldı.  
- **Shader Seçimi:** `Simple Lit` shader kullanarak mobil cihazlarda performans artırıldı.  
- **Shadow Optimizasyonu:**  
  - Gerçek zamanlı ışık ve gölgeler kapatıldı.  
  - **Fake gölgeler** ile görsel kalite korundu.  
- **Mesh Optimizasyonu:**  
  - **Reverse engineering** ile alınan bazı varlıkların vertex sayıları azaltıldı.  
  - Örnek: `Helmet01` modeli **40K vertex** içeriyordu, **MantisLODEditor** ile **400 vertex**'e düşürüldü.  
- **Bellek ve Dosya Boyutu Optimizasyonu:**  
  - Görseller için sıkıştırma ve `max size` ayarlamaları yapıldı.  
```  
