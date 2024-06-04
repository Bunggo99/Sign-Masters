using UnityEngine;

public static class WordList
{
    #region Randomize Word

    public static string RandomizeWord()
    {
        int randIndex = Random.Range(0, WordArray.Length);
        return WordArray[randIndex];
    }

    public static string RandomizeLetters(int length, char currLetter)
    {
        string word = "";
        for (int i = 0; i < length; i++)
        {
            char randchara = (char)Random.Range('A', currLetter + 1);
            word += randchara;
        }
        
        if(!word.Contains(currLetter))
        {
            int randIndex = Random.Range(0, word.Length);
            word = word.Remove(randIndex, 1);
            word = word.Insert(randIndex, currLetter.ToString());
        }
        return word;
    }

    public static char RandomizeCharacter(char currLetter)
    {
        char randchara = (char)Random.Range('A', currLetter + 1);
        return randchara;
    }

    #endregion

    #region Word Array

    private static readonly string[] WordArray = 
    {
        "ahli",
        "alam",
        "apel",
        "awan",
        "baju",
        "batu",
        "besi",
        "biru",
        "bola",
        "buku",
        "bumi",
        "busa",
        "cara",
        "dagu",
        "dasi",
        "emas",
        "foto",
        "gaun",
        "gigi",
        "halo",
        "hama",
        "hari",
        "ikan",
        "jala",
        "jari",
        "kaca",
        "kain",
        "kaki",
        "kuku",
        "laci",
        "lari",
        "leci",
        "lidi",
        "lucu",
        "mata",
        "meja",
        "nada",
        "nadi",
        "nasi",
        "obat",
        "obor",
        "paku",
        "palu",
        "qari",
        "seni",
        "rabu",
        "ragu",
        "raja",
        "siku",
        "tali",
        "uang",
        "uban",
        "ubin",
        "ukir",
        "ukur",
        "vena",
        "vila",
        "visi",
        "voli",
        "wali",
        "yang",
        "yoga",
        "yoyo",
        "zona",
    };

    #endregion
}
