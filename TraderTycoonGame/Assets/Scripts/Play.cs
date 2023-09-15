using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;


public class Play : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI dateText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI emiText;
    [SerializeField] TMP_InputField loanText;
    [SerializeField] TMP_InputField rawMaterialText;
    [SerializeField] TMP_InputField employeeNumberText;
    [SerializeField] TMP_InputField sellingPriceInputText;
    [SerializeField] TextMeshProUGUI rawMaterialInventoryText;
    [SerializeField] TextMeshProUGUI employeeInventoryText;
    [SerializeField] TextMeshProUGUI productInventoryText;
    [SerializeField] TextMeshProUGUI sellingPriceText;
    [SerializeField] TextMeshProUGUI marketPriceText;
    int day, month, year, money, loan, cutDay, emi, rawMaterial, numberE, produce, product, sellingPrice, marketPrice;
    bool[] macLock, macP;
    public DetailsObject detailsObj;
    [SerializeField] TextAsset detailsFileAsset;
    [System.Serializable]
    public class DetailsFormat
    {
        public int Day;
        public int Month;
        public int Year;
        public int Money;
        public int Loan;
        public int Emi;
        public int CutDay;
        public int RawMaterial;
        public int NumberE;
        public bool[] MacLock;
        public bool[] MacP;
        public int Produce;
        public int Product;
        public int SellingPrice;
        public int MarketPrice;
    }
    [System.Serializable]
    public class DetailsObject
    {
        public DetailsFormat[] Data;
    }
    
    void LoadFromJson()
    {
        string path = Application.persistentDataPath + "/Details.json";
        if(File.Exists(path))
        {
            detailsObj = JsonUtility.FromJson<DetailsObject>(File.ReadAllText(path));
        }
        else
        {
            detailsObj = JsonUtility.FromJson<DetailsObject>(detailsFileAsset.text);  
        }
    } 
    void SaveToJson()
    {
        UpdateDetails();
        string path = Application.persistentDataPath + "/Details.json";
        File.WriteAllText(path,JsonUtility.ToJson(detailsObj));
    }
    void Initialisation()
    {
        day = detailsObj.Data[0].Day;
        month = detailsObj.Data[0].Month;
        year = detailsObj.Data[0].Year;
        money = detailsObj.Data[0].Money;
        loan= detailsObj.Data[0].Loan;
        emi= detailsObj.Data[0].Emi;
        cutDay= detailsObj.Data[0].CutDay;
        rawMaterial= detailsObj.Data[0].RawMaterial;
        numberE= detailsObj.Data[0].NumberE;
        macLock= detailsObj.Data[0].MacLock;
        macP= detailsObj.Data[0].MacP;
        produce= detailsObj.Data[0].Produce;
        product= detailsObj.Data[0].Product;
        sellingPrice = detailsObj.Data[0].SellingPrice;
        marketPrice= detailsObj.Data[0].MarketPrice;
    }
    void UpdateDetails()
    {
        detailsObj.Data[0].Day = day;
        detailsObj.Data[0].Month = month;
        detailsObj.Data[0].Year = year;
        detailsObj.Data[0].Money= money;
        detailsObj.Data[0].Loan= loan;
        detailsObj.Data[0].Emi= emi;
        detailsObj.Data[0].CutDay= cutDay;
        detailsObj.Data[0].RawMaterial= rawMaterial;
        detailsObj.Data[0].NumberE= numberE;
        detailsObj.Data[0].MacLock = macLock;
        detailsObj.Data[0].MacP= macP;
        detailsObj.Data[0].Produce= produce;
        detailsObj.Data[0].Product= product;
        detailsObj.Data[0].SellingPrice= sellingPrice;
        detailsObj.Data[0].MarketPrice= marketPrice;
    }
    void VisibilityMachineElements()
    {
        GameObject[] macButtonsR = GameObject.FindGameObjectsWithTag("MacButtons");
        GameObject[] macTextR = GameObject.FindGameObjectsWithTag("MacTexts");
        GameObject[] macButtons=new GameObject[3];
        GameObject[] macText = new GameObject[3];
        foreach (GameObject g in macButtonsR)
        {
            if (g.name == "Mac1Button")
            {
                macButtons[0] = g;
            }
            if(g.name == "Mac2Button")
            {
                macButtons[1] = g;
            }
            if(g.name == "Mac3Button")
            {
                macButtons[2] = g;
            }
        }
        foreach(GameObject g in macTextR)
        {
            if(g.name == "PMac1")
            {
                macText[0]= g;
            }
            if(g.name == "PMac2")
            {
                macText[1]= g;
            }
            if(g.name == "PMac3")
            {
                macText[2]= g;
            }
        }
        for(int i=0;i<macButtons.Length; i++)
        {
            CanvasGroup cg = macButtons[i].GetComponent<CanvasGroup>();
            CanvasGroup cg2= macText[i].GetComponent<CanvasGroup>();
            if (macLock[i])
            {
                cg.interactable = false;
                cg.alpha = 0;
            }
            else
            {
                cg.interactable = !macP[i];
                if (macP[i])
                {
                    cg.alpha = 0;
                    cg2.alpha = 0;
                }
                else
                {
                    cg2.alpha = 1;
                    cg.alpha = 1;
                }
            }
        }
    }
    void Start()
    {
        LoadFromJson();
        Initialisation();
        VisibilityMachineElements();
        if (marketPrice == 0)
        {
            marketPrice = PriceCalculator();
        }
        marketPriceText.text=marketPrice.ToString();
        productInventoryText.text = product.ToString();
    }
    void UpdateDate()
    {
        int totalDays;
        if (month <= 7)
        {
            if (month == 2)
            {
                totalDays = 28;
            }
            else if (month % 2 == 0)
            {
                totalDays = 30;
            }
            else
            {
                totalDays = 31;
            }
        }
        else
        {
            if(month%2==0)
            {
                totalDays = 31;
            }
            else
            {
                totalDays= 30;
            }
        }
        day += 1;
        if (day > totalDays)
        {
            day = 1;
            month += 1;
            if (month > 12)
            {
                month = 1;
                year += 1;
            }
        }
    }
    string MoneyParser(int money)
    {
        if (money>=1000000)
        {
            return ((money / 1000000.0).ToString("F3")+"M");
        }
        else if (money>=1000)
        {
            return ((money/1000.0).ToString("F3")+"K");
        }
        else
        {
            return money.ToString();
        }
    }
    void UpdateLoan(bool isEmiU)
    {
        if (isEmiU==false)
        {
            if (int.Parse(loanText.text) >= 1000 & int.Parse(loanText.text)<=30000)
            {
                loan+=int.Parse(loanText.text);
                money += int.Parse(loanText.text);
                UpdateEmi();
            }
        }
        loanText.text = string.Empty;
        if (isEmiU)
        {
            loan -= emi;
        }
    }
    void PrintData()
    {
        dateText.text=day.ToString()+"/"+month.ToString()+"/"+year.ToString();
        moneyText.text="$"+MoneyParser(money);
        emiText.text="$"+emi.ToString();
        rawMaterialInventoryText.text=rawMaterial.ToString();
        employeeInventoryText.text=numberE.ToString();
        
    }
    public void DateButton()
    {
        UpdateDate();
        UpdateLoan(true);
        UpdateMoney();
        ProduceManager();
        IncreaseSalePrice();
        LoanChecker();
        UpgradeMachineUnlock();
        productInventoryText.text = product.ToString();
        SaveToJson();
        marketPrice = PriceCalculator();
        marketPriceText.text = marketPrice.ToString();
         
        LoseCheck();
    }
    void UpgradeMachineUnlock()
    {
        
        if (macLock[1])
        {
            if (day == 1 && month == 6 && year == 2023)
            {
                macLock[1] = false;
                VisibilityMachineElements();
            }
        }
        if (macLock[2])
        {
            if (day == 1 && month == 6 && year == 2024)
            {
                macLock[2] = false;
                VisibilityMachineElements();
            }
        }

    }
    void UpdateEmi()
    {
        if (loan == 0)
        {
            emi = loan / 365;
            cutDay += emi;
        }
        else
        {
            int l=int.Parse(loanText.text);
            int em = l / 365;
            emi += em;
            cutDay += em;
        }
    }
    void UpdateMoney()
    {
        money -= cutDay;
    }
    public void LoanButton()
    {
        UpdateLoan(false);
        SaveToJson();
    }
    public void RawMaterialButton()
    {
        BuyRawMaterial();
        SaveToJson();
    }
    public void EmployeeButton()
    {
        EmployeeManager(false);
        
        SaveToJson();
    }
    public void EmployeeButton2()
    {
        EmployeeManager(true);
        
        SaveToJson();
    }
    public void MachineBuyButton1()
    {
        BuyMachine(1);
    }
    public void MachineBuyButton2()
    {
        BuyMachine(2);
    }
    public void MachineBuyButton3()
    {
        BuyMachine(3);
    }
    
    void ProduceManager()
    {
        int limit = 0;
        if (macP[0])
        {
            limit = 10;
        }
        if (macP[1])
        {
            limit = 20;
        }
        if (macP[2])
        {
            limit = 30;
        }
        int r = rawMaterial;
        int e = numberE;
        if (2 * e > limit)
        {
            if (r > limit)
            {
                produce = limit;
            }
            else
            {
                produce = r;
            }
        }
        else
        {
            if (r > 2 * e)
            {
                produce = 2 * e;
            }
            else
            {
                produce = r;
            }
        }
        product += produce;
        rawMaterial-=produce;
    }
    void BuyMachine(int mid)
    {
        bool isBuy=false;
        if (mid == 2)
        {
            if (macP[0])
            {
                isBuy = true;
            }
            else
            {
                isBuy= false;
            }
        }
        else if (mid==3)
        {
            if (macP[1])
            {
                isBuy = true;
            }
            else
            {
                isBuy = false;
            }
        }
        else
        {
            isBuy= true;
        }
        if (money >= 10000 * mid && isBuy)
        {
            money -= 10000 * mid;
            macP[mid - 1] = true;
            VisibilityMachineElements();
            SaveToJson();
        }
    }
   
    void LoanChecker()
    {
        if (loan < 0)
        {
            loan = 0;
        }
        if (loan == 0)
        {
            cutDay -= emi;
            emi = 0;
        }
    }
    void BuyRawMaterial()
    {
        int amount = int.Parse(rawMaterialText.text)*2;
        if (money >= amount)
        {
            money -= amount;
            rawMaterial += int.Parse(rawMaterialText.text);
        }
        rawMaterialText.text=string.Empty;
    }
    void EmployeeManager(bool isCut)
    {
        if (!isCut)
        {
            int amount = int.Parse(employeeNumberText.text) * 10;
            if (money >= amount)
            {
                numberE += int.Parse(employeeNumberText.text);
                cutDay += amount;
            }
        }
        else
        {
            int cut = int.Parse(employeeNumberText.text);
            if (cut >= numberE)
            {
                numberE = 0;
            }
            else
            {
                numberE -= cut;
            }
        }
        employeeNumberText.text=string.Empty;
    }
    void LoseCheck()
    {
        if (loan!=0 & cutDay > money)
        {
            string path = Application.persistentDataPath + "/Details.json";
            File.WriteAllText(path,JsonUtility.ToJson(JsonUtility.FromJson<DetailsObject>(detailsFileAsset.text)));
            SceneManager.LoadScene(0);
        }
    }
    public void SellingPriceButton()
    {
        SellingPriceSet();
    }
    public void ResetButton()
    {
        string path = Application.persistentDataPath + "/Details.json";
        File.WriteAllText(path, JsonUtility.ToJson(JsonUtility.FromJson<DetailsObject>(detailsFileAsset.text)));
        SceneManager.LoadScene(0);
    }
    void SellingPriceSet()
    {
        int p = int.Parse(sellingPriceInputText.text);
        sellingPrice = p;
        sellingPriceText.text=p.ToString();
        
    }
    void IncreaseSalePrice()
    {
        int diff = sellingPrice - marketPrice;
        float sell = 1;
        if (diff > 2)
        {
            sell = 0;
        }
        if (diff == 2)
        {
            sell = 0.5f;
        }
        if (diff == 1)
        {
            sell = 0.8f;
        }
        int proToSell = Mathf.FloorToInt(product * sell);
        int amount = sellingPrice * proToSell;
        {
            money += amount;
            product -= proToSell;
        }
        
        
    }
    int PriceCalculator()
    {
        marketPrice = Random.Range(9, 13);
        return marketPrice;
    }
    // Update is called once per frame
    void Update()
    {
        PrintData();
     }
}
