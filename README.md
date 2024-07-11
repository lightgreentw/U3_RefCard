# U3 Reference Card

### 歡迎來到創世紀三的奇幻國度，沒想到這款1983年的遊戲讓我著迷了，不僅重回初學者的狀態邊遊玩邊閱讀說明書外，還為了降低學習曲線，並能夠順暢的遊歷索沙尼亞，重新繪製了鍵盤對照表。
### 我需要一個能夠時時開在旁邊的鍵盤對照表快速參照，而且還能夠隨意放大縮小，並隨意放置；於是依照以上的想法我寫了一個很簡單的工具，而且不只是對照表，還能放置攻略、地圖等等，這樣就很簡單在螢幕上拖移圖片，還能將迷宮地圖變成透明疊放在遊戲上更簡便的參考。
_特別說明：參考表中的中文不是翻譯，而是以我參考精訊出版的創世紀三及軟體世界出版的創世紀一二三典藏版說明書，以及各大攻略網站最後自己能快速理解的語句編成，鍵盤指令也是以應用區域的方式排列。_
_如有超譯或會錯意的地方請指教。_
<p align = "center">
<img src="https://github.com/lightgreentw/U3_RefCard/blob/master/ReadmePIC_01.png" width=70%>
</p>

# 安裝及執行
1. 下載Zip檔後，直接解壓縮後放置在任意路徑下。
2. 找到並雙擊U3_RefCard.exe執行檔。
3. 執行後桌面上會出現兩張預設的鍵盤對照表，在右下角的工作列通知區有常駐的圖示，單擊滑鼠左鍵即顯示操作選單。

# 操作方式
1. 滑鼠操作（以下操作都是在圖片上）

|            按鍵加滑鼠            |      動作      |
|---------------------------------|:--------------:|
|長按滑鼠左鍵 + 滑動滑鼠            |     移動圖片   |
|Ctrl + 長按滑鼠左鍵 + 左上右下滑動 |     圖片縮放   |
|Alt + 長按滑鼠左鍵 + 左右滑動      |     圖片透明   |
|Alt + 雙擊滑鼠左鍵                |   圖片色彩反向  |
|Shift + 雙擊滑鼠右鍵              |   還原圖片狀態  |

2. 工作列通知區圖示
<p align = "left">
<img src="https://github.com/lightgreentw/U3_RefCard/blob/master/ReadmePIC_02.png" width=40%>
</p>

# 設定
所有設定都是以文字方式紀錄的 U3_RefCard.INI 檔，預設只有兩個節，其他參考圖片檔請依參數設定。

```
;參考表的索引
;必須對應參考表的數量
[RefCardIndex]
Count=4

;參考表的節，以 [RefCard] 為開頭，後面數字為流水號(必須連續)，以此類推。
;參數如下:
;    Title 參考表名稱。 
;    RefFile 參考表檔案名稱，適用PNG, JPG圖片檔，檔案必須放在執行檔的路徑下。
;    Top 紀錄對應螢幕的X軸，初次設定為0即可。
;    Left 紀錄對應螢幕的Y軸，初次設定為0即可。
;    Height 紀錄參考表圖片高，初次設定為0即可。
;    Width 紀錄參考表圖片寬，初次設定為0即可，Height及Width皆為0時圖片會回到初始狀態。
;    Opacity 透明度，1是不透明，0是完全透明，0.5是半透明。
;    InvertColor 反向顏色，如果地圖是黑色底白色線條在為透明的狀態下會比較清楚。
;    DefaultOn 預設開啟，程式啟動時即打開的圖片。
[RefCard1]
Title=Reference Card 1
RefFile=RefCard_01.png
Top=199
Left=44
Height=587
Width=468
Opacity=0.782000005245209
InvertColor=False
DefaultOn=true

[RefCard2]
Title=Reference Card 2
RefFile=RefCard_02.png
Top=43
Left=41
Height=126
Width=1166
Opacity=1
InvertColor=False
DefaultOn=true
```

## Reference
* Ultima III: Exodus is copyright Origin System.
* DOSBox-X is an open-source DOS emulator https://dosbox-x.com/
* Ultima 3 Upagrde in https://exodus.voyd.net/
* 文中提到的說明書皆來自骨灰集散地的說明書補完計畫 https://boneash.oldgame.tw/
* 文中提到的攻略及地圖網站
  * _LairWare_ https://www.lairware.com/ultima3/ ,
  * _Gamer Corner Guides_ https://guides.gamercorner.net/ultimaiii/
