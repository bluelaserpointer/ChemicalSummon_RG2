using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public static class ChemicalSummonEditor
{
    private static List<string> unknownNames = new List<string>();
    /// <summary>
    /// 读取道具标头表自动生成ScriptableObject
    /// </summary>
    [MenuItem("ChemicalSummon/Load ItemHeader From Excel")]
    private static void LoadItemHeaderExcel()
    {
        DataSet result = ReadExcelFromStreamingAsset("ItemHeader.xlsx");
        if (result == null)
            return;
        int newCreatedCount = 0;
        int updatedCount = 0;
        foreach (DataTable table in result.Tables)
        {
            int rows = table.Rows.Count;
            for (int row = 1; row < rows; row++)
            {
                DataRow rowData = table.Rows[row];
                string itemHeaderFileName = rowData[0].ToString();
                ItemHeader itemHeader = ItemHeader.GetByName(itemHeaderFileName);
                bool newCreated = itemHeader == null;
                if (newCreated)
                {
                    itemHeader = ScriptableObject.CreateInstance<ItemHeader>();
                }
                itemHeader.name.defaultString = itemHeaderFileName;
                itemHeader.name.PutSentence_EmptyStrMeansRemove(Language.English, rowData[1].ToString());
                itemHeader.name.PutSentence_EmptyStrMeansRemove(Language.Chinese, rowData[2].ToString());
                itemHeader.name.PutSentence_EmptyStrMeansRemove(Language.Japanese, rowData[3].ToString());
                itemHeader.description.PutSentence_EmptyStrMeansRemove(Language.English, rowData[4].ToString());
                itemHeader.description.PutSentence_EmptyStrMeansRemove(Language.Chinese, rowData[5].ToString());
                itemHeader.description.PutSentence_EmptyStrMeansRemove(Language.Japanese, rowData[6].ToString());
                if (newCreated)
                {
                    AssetDatabase.CreateAsset(itemHeader, General.ResourcePath.Absolute + General.ResourcePath.Item + itemHeaderFileName + ".asset");
                    ++newCreatedCount;
                }
                else
                {
                    EditorUtility.SetDirty(itemHeader);
                    ++updatedCount;
                }
            }
        }
        AssetDatabase.SaveAssets(); //存储资源
        AssetDatabase.Refresh(); //刷新
        Debug.Log("ItemHeaderAssetsCreated. updatedCount: " + updatedCount + ", newCreated: " + newCreatedCount);
    }
    /// <summary>
    /// 读取关卡标头表自动生成ScriptableObject
    /// </summary>
    [MenuItem("ChemicalSummon/Load StageHeader From Excel")]
    private static void LoadStageHeaderExcel()
    {
        DataSet result = ReadExcelFromStreamingAsset("StageHeader.xlsx");
        if (result == null)
            return;
        int newCreatedCount = 0;
        int updatedCount = 0;
        foreach (DataTable table in result.Tables)
        {
            int rows = table.Rows.Count;
            for (int row = 1; row < rows; row++)
            {
                DataRow rowData = table.Rows[row];
                string stageHeaderID = rowData[0].ToString();
                StageHeader stageHeader = StageHeader.GetByName(stageHeaderID);
                bool newCreated = stageHeader == null;
                if (newCreated)
                {
                    stageHeader = ScriptableObject.CreateInstance<StageHeader>();
                }
                stageHeader.id = stageHeaderID;
                stageHeader.difficulty = ToInt(rowData[1].ToString());
                stageHeader.name.defaultString = stageHeaderID;
                stageHeader.name.PutSentence_EmptyStrMeansRemove(Language.Chinese, rowData[2].ToString());
                stageHeader.name.PutSentence_EmptyStrMeansRemove(Language.Japanese, rowData[3].ToString());
                stageHeader.name.PutSentence_EmptyStrMeansRemove(Language.English, rowData[4].ToString());
                stageHeader.description.PutSentence_EmptyStrMeansRemove(Language.Chinese, rowData[2].ToString());
                stageHeader.description.PutSentence_EmptyStrMeansRemove(Language.Japanese, rowData[3].ToString());
                stageHeader.description.PutSentence_EmptyStrMeansRemove(Language.English, rowData[4].ToString());
                if (newCreated)
                {
                    AssetDatabase.CreateAsset(stageHeader, General.ResourcePath.Absolute + General.ResourcePath.Stage + stageHeaderID + ".asset");
                    ++newCreatedCount;
                }
                else
                {
                    EditorUtility.SetDirty(stageHeader);
                    ++updatedCount;
                }
            }
        }
        AssetDatabase.SaveAssets(); //存储资源
        AssetDatabase.Refresh(); //刷新
        Debug.Log("StageHeaderAssetsCreated. updatedCount: " + updatedCount + ", newCreated: " + newCreatedCount);
    }
    /// <summary>
    /// 读取可翻译句子表自动生成ScriptableObject
    /// </summary>
    [MenuItem("ChemicalSummon/Load TranslatableSentence From Excel")]
    private static void LoadTranslatableSentenceExcel()
    {
        DataSet result = ReadExcelFromStreamingAsset("TranslatableSentence.xlsx");
        if (result == null)
            return;
        int newCreatedCount = 0;
        int updatedCount = 0;
        foreach (DataTable table in result.Tables)
        {
            int rows = table.Rows.Count;
            for (int row = 1; row < rows; row++)
            {
                DataRow rowData = table.Rows[row];
                string sentenceName = rowData[0].ToString();
                TranslatableSentenceSO sentence = Resources.Load<TranslatableSentenceSO>("TranslatableSentence/" + sentenceName);
                bool newCreated = sentence == null;
                if (newCreated)
                {
                    sentence = ScriptableObject.CreateInstance<TranslatableSentenceSO>();
                }
                sentence.sentence.defaultString = sentenceName;
                sentence.sentence.PutSentence_EmptyStrMeansRemove(Language.English, rowData[1].ToString());
                sentence.sentence.PutSentence_EmptyStrMeansRemove(Language.Chinese, rowData[2].ToString());
                sentence.sentence.PutSentence_EmptyStrMeansRemove(Language.Japanese, rowData[3].ToString());
                if (newCreated)
                {
                    AssetDatabase.CreateAsset(sentence, General.ResourcePath.Absolute + "TranslatableSentence/" + sentenceName + ".asset");
                    ++newCreatedCount;
                }
                else
                {
                    EditorUtility.SetDirty(sentence);
                    ++updatedCount;
                }
            }
        }
        AssetDatabase.SaveAssets(); //存储资源
        AssetDatabase.Refresh(); //刷新
        Debug.Log("TranslatableSentenceAssetsCreated. updatedCount: " + updatedCount + ", newCreated: " + newCreatedCount);
    }
    /// <summary>
    /// 读取角色表自动生成ScriptableObject
    /// </summary>
    [MenuItem("ChemicalSummon/Load Character From Excel")]
    private static void LoadCharacterExcel()
    {
        Dictionary<Language, int> languagePos = new Dictionary<Language, int>();
        languagePos.Add(Language.Chinese, 0);
        languagePos.Add(Language.Japanese, 1);
        languagePos.Add(Language.English, 2);
        DataSet result = ReadExcelFromStreamingAsset("Character.xlsx");
        if (result == null)
            return;
        DataTable table = result.Tables[0];
        int rows = table.Rows.Count;
        int cols = table.Columns.Count;
        int newCreatedCount = 0;
        int updatedCount = 0;
        Character character = null;
        string characterName = null;
        bool newCreated = false;
        //input speakType
        List<Character.SpeakType> colSpeakType = new List<Character.SpeakType>();
        DataRow firstRow = table.Rows[0];
        for (int i = 2; i < cols; ++i)
        {
            Character.SpeakType speakType = (Character.SpeakType)Enum.Parse(typeof(Character.SpeakType), firstRow[i].ToString());
            colSpeakType.Add(speakType);
        }
        Action saveCharacter = () =>
        {
            if (character != null) //save last character
            {
                if (newCreated)
                {
                    AssetDatabase.CreateAsset(character, General.ResourcePath.Absolute + General.ResourcePath.Character + characterName + ".asset");
                    ++newCreatedCount;
                }
                else
                {
                    EditorUtility.SetDirty(character);
                    ++updatedCount;
                }
            }
        };
        for (int row = 0; row < rows; row++)
        {
            DataRow rowData = table.Rows[row];
            Language language;
            string header = rowData[0].ToString();
            if (!Enum.TryParse(header, out language)) //new character block
            {
                saveCharacter.Invoke();
                character = Character.GetByName(header);
                character.name.defaultString = characterName = header;
                newCreated = character == null;
                if (newCreated)
                {
                    character = ScriptableObject.CreateInstance<Character>();
                }
            }
            else //new language row
            {
                if (character.name == null)
                    character.name = new TranslatableSentence();
                character.name.PutSentence_EmptyStrMeansRemove(language, rowData[1].ToString());
                for (int i = 2; i < cols; ++i)
                {
                    Character.SpeakType speakType = colSpeakType[i - 2];
                    TranslatableSentence sentence = null;
                    foreach(var pair in character.speaks)
                    {
                        if(pair.speakType.Equals(speakType))
                        {
                            sentence = pair.translatableSentence;
                            break;
                        }
                    }
                    if(sentence == null)
                    {
                        character.speaks.Add(new Character.SpeakTypeAndSentence(speakType, sentence = new TranslatableSentence()));
                    }
                    sentence.PutSentence_EmptyStrMeansRemove(language, rowData[i].ToString());
                    if (language.Equals(Language.English))
                        sentence.defaultString = rowData[i].ToString();
                }
            }
            character.initialHP = 65;
            character.faceIcon = Resources.Load<Sprite>(General.ResourcePath.Character + "FaceIcon/" + characterName);
            character.portrait = Resources.Load<Sprite>(General.ResourcePath.Character + "Portrait/" + characterName);
        }
        saveCharacter.Invoke(); //save last character
        AssetDatabase.SaveAssets(); //存储资源
        AssetDatabase.Refresh(); //刷新
        Debug.Log("CharacterAssetsCreated. updatedCount: " + updatedCount + ", newCreated: " + newCreatedCount);
    }
    /// <summary>
    /// 读取反应式表自动生成ScriptableObject
    /// </summary>
    [MenuItem("ChemicalSummon/Load Reaction From Excel")]
    private static void LoadReactionExcel()
    {
        unknownNames.Clear();
        DataSet result = ReadExcelFromStreamingAsset("Reaction.xlsx");
        if (result == null)
            return;
        int newCreatedCount = 0;
        int updatedCount = 0;
        foreach (DataTable table in result.Tables)
        {
            int rows = table.Rows.Count;
            for (int row = 1; row < rows; row++)
            {
                DataRow rowData = table.Rows[row];
                string formulaStr = rowData[0].ToString();
                Reaction reaction = Reaction.LoadFromResources(formulaStr);
                bool newCreated = reaction == null;
                if (newCreated)
                {
                    reaction = ScriptableObject.CreateInstance<Reaction>();
                }
                reaction.formula = formulaStr;
                string[] strs = formulaStr.Split('=');
                Reaction.Stats stats = new Reaction.Stats();
                stats.leftSubstances = StrToSubstanceAndAmount(strs[0]);
                stats.catalysts = StrToSubstanceAndAmount(strs[1]);
                stats.rightSubstances = StrToSubstanceAndAmount(strs[2]);
                stats.heatRequire = ToInt(rowData[1].ToString());
                stats.electricRequire = ToInt(rowData[2].ToString());
                stats.vigorousness = ToInt(rowData[3].ToString());
                stats.heat = ToInt(rowData[4].ToString());
                stats.electric = ToInt(rowData[5].ToString());
                reaction.initialStats = stats;
                if (newCreated)
                {
                    AssetDatabase.CreateAsset(reaction, General.ResourcePath.Absolute + General.ResourcePath.Reaction + Substance.RemoveFormulaCaseConflict(formulaStr) + ".asset");
                    ++newCreatedCount;
                }
                else
                {
                    EditorUtility.SetDirty(reaction);
                    ++updatedCount;
                }
            }
        }
        AssetDatabase.SaveAssets(); //存储资源
        AssetDatabase.Refresh(); //刷新
        Debug.Log("ReactionAssetsCreated. updatedCount: " + updatedCount + ", newCreated: " + newCreatedCount);
        if(unknownNames.Count > 0)
        {
            string names = "";
            unknownNames.ForEach(name => names += "\r\n" + name);
            Debug.LogWarning("UnknownNames: " + unknownNames.Count + names);
        }
    }
    /// <summary>
    /// 读取物质表自动生成ScriptableObject
    /// </summary>
    [MenuItem("ChemicalSummon/Load Substance From Excel")]
    private static void LoadSubstanceExcel()
    {
        DataSet result = ReadExcelFromStreamingAsset("Substance.xlsx");
        if (result == null)
            return;
        int newCreatedCount = 0;
        int updatedCount = 0;
        List<string> noImageSubstancesName = new List<string>();
        foreach (DataTable table in result.Tables)
        {
            bool isFirstLine = true;
            foreach(DataRow row in table.Rows)
            {
                if(isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }
                string substanceName = row[0].ToString();
                Substance substance = Substance.LoadFromResources(substanceName);
                bool newCreated = substance == null;
                if (newCreated)
                {
                    substance = ScriptableObject.CreateInstance<Substance>();
                }
                else
                {
                    substance.elements.Clear();
                }
                substance.formula = substanceName;
                //analyze compounds from molecular name
                string molecularStr = row[1].ToString();
                if (molecularStr.Length == 0) //when structual name and molecular name are same
                    molecularStr = substanceName;
                string tmpElementName = "";
                string lastLetter = "";
                bool lastIsNumber = false;
                foreach (char letter in molecularStr)
                {
                    if (lastLetter.Length == 0)
                    {
                        lastLetter += letter;
                    }
                    else if (char.IsUpper(letter))
                    {
                        if (!lastIsNumber) // exp. CO, (NH4)2SO4
                        {
                            AvoidNull(Element.GetByNameWithWarn(lastLetter), element => substance.elements.Add(element));
                        }
                        else //exp. H2O
                        {
                            AvoidNull(Element.GetByNameWithWarn(tmpElementName), element => substance.elements.Add(element, ToInt(lastLetter)));
                        }
                        lastLetter = letter.ToString();
                        lastIsNumber = false;
                    }
                    else if (char.IsNumber(letter))
                    {
                        if (!lastIsNumber) // exp. H2O
                        {
                            tmpElementName = lastLetter;
                            lastLetter = letter.ToString();
                        }
                        else
                        {
                            lastLetter += letter;
                        }
                        lastIsNumber = true;
                    }
                    else if (char.IsLower(letter))
                    {
                        lastLetter += letter;
                    }
                    else
                    {
                        Debug.Log("encounted unknown character: " + letter);
                    }
                }
                //end phase
                if (!lastIsNumber) // exp. Fe
                {
                    if (lastLetter.Equals("?")) //Phenomenons does not have elements
                        substance.isPhenomenon = true;
                    else
                    {
                        AvoidNull(Element.GetByNameWithWarn(lastLetter), element => substance.elements.Add(element));
                        substance.isPhenomenon = false;
                    }
                }
                else //exp. H2
                {
                    AvoidNull(Element.GetByNameWithWarn(tmpElementName), element => substance.elements.Add(element, ToInt(lastLetter)));
                    substance.isPhenomenon = false;
                }
                substance.rank = ToInt(row[2].ToString());
                if (substance.rank == 0)
                    substance.rank = 3;
                substance.atk = ToInt(row[3].ToString());
                substance.meltingPoint = ToInt(row[4].ToString());
                substance.boilingPoint = ToInt(row[5].ToString());
                substance.name.defaultString = substanceName;
                substance.name.PutSentence_EmptyStrMeansRemove(Language.Chinese, row[6].ToString());
                substance.name.PutSentence_EmptyStrMeansRemove(Language.Japanese, row[7].ToString());
                substance.name.PutSentence_EmptyStrMeansRemove(Language.English, row[8].ToString());
                substance.abilityPrefab = CardAbility.LoadFromResources(substanceName);
                substance.description.defaultString = "";
                substance.description.PutSentence_EmptyStrMeansRemove(Language.Chinese, row[9].ToString());
                substance.description.PutSentence_EmptyStrMeansRemove(Language.Japanese, row[10].ToString());
                substance.description.PutSentence_EmptyStrMeansRemove(Language.English, row[11].ToString());
                string substanceNameWithoutCaseConflict = Substance.RemoveFormulaCaseConflict(substanceName);
                substance.image = Resources.Load<Sprite>(General.ResourcePath.CardSprite + substanceNameWithoutCaseConflict);
                if (substance.image == null)
                    noImageSubstancesName.Add(substance.formula);
                if (newCreated)
                {
                     AssetDatabase.CreateAsset(substance, General.ResourcePath.Absolute + General.ResourcePath.Substance + substanceNameWithoutCaseConflict + ".asset");
                    ++newCreatedCount;
                }
                else
                {
                    EditorUtility.SetDirty(substance);
                    ++updatedCount;
                }
            }
        }
        AssetDatabase.SaveAssets(); //存储资源
        AssetDatabase.Refresh(); //刷新
        Debug.Log("SubstanceAssetsCreated. updatedCount: " + updatedCount + ", newCreated: " + newCreatedCount);
        if(noImageSubstancesName.Count > 0)
        {
            string str = "";
            noImageSubstancesName.Sort();
            noImageSubstancesName.ForEach(each => str += each + "\t");
            Debug.LogWarning("Those substances doesn\'t have image (" + noImageSubstancesName.Count + "):\r\n" + str);
        }
    }
    /// <summary>
    /// 读取元素表自动生成ScriptableObject
    /// </summary>
    /// <returns></returns>
    [MenuItem("ChemicalSummon/Load Element From Excel")]
    private static void LoadElementExcel()
    {
        DataSet result = ReadExcelFromStreamingAsset("Element.xlsx");
        if (result == null)
            return;
        DataTable table = result.Tables[0];
        int rows = table.Rows.Count;
        int newCreatedCount = 0;
        int updatedCount = 0;
        for (int row = 1; row < rows; row++)
        {
            DataRow rowData = table.Rows[row];
            string elementName = rowData[0].ToString();
            Element element = Element.LoadFromResources(elementName);
            bool newCreated = element == null;
            if (newCreated)
            {
                element = ScriptableObject.CreateInstance<Element>();
            }
            element.chemicalSymbol = elementName;
            element.atom = ToInt(rowData[1].ToString());
            element.mol = ToInt(rowData[2].ToString());
            element.name.defaultString = elementName;
            element.name.PutSentence_EmptyStrMeansRemove(Language.Chinese, rowData[3].ToString());
            element.name.PutSentence_EmptyStrMeansRemove(Language.Japanese, rowData[4].ToString());
            element.name.PutSentence_EmptyStrMeansRemove(Language.English, rowData[5].ToString());
            if (newCreated)
            {
                AssetDatabase.CreateAsset(element, General.ResourcePath.Absolute + General.ResourcePath.Element + elementName + ".asset");
                ++newCreatedCount;
            }
            else
            {
                EditorUtility.SetDirty(element);
                ++updatedCount;
            }
        }
        AssetDatabase.SaveAssets(); //存储资源
        AssetDatabase.Refresh(); //刷新
        Debug.Log("ElementAssetsCreated. updatedCount: " + updatedCount + ", newCreated: " + newCreatedCount);
    }
    /// <summary>
    /// 读取道具标头表自动生成ScriptableObject
    /// </summary>
    [MenuItem("ChemicalSummon/Load MagicCard From Excel")]
    private static void LoadMagicCardExcel()
    {
        DataSet result = ReadExcelFromStreamingAsset("MagicCard.xlsx");
        if (result == null)
            return;
        int newCreatedCount = 0;
        int updatedCount = 0;
        foreach (DataTable table in result.Tables)
        {
            int rows = table.Rows.Count;
            for (int row = 1; row < rows; row++)
            {
                DataRow rowData = table.Rows[row];
                string magicFileName = rowData[0].ToString();
                Magic magic = Magic.LoadFromResources(magicFileName);
                bool newCreated = magic == null;
                if (newCreated)
                {
                    magic = ScriptableObject.CreateInstance<Magic>();
                }
                magic.name.defaultString = magicFileName;
                magic.name.PutSentence_EmptyStrMeansRemove(Language.English, rowData[0].ToString());
                magic.name.PutSentence_EmptyStrMeansRemove(Language.Chinese, rowData[1].ToString());
                magic.name.PutSentence_EmptyStrMeansRemove(Language.Japanese, rowData[2].ToString());
                magic.description.PutSentence_EmptyStrMeansRemove(Language.English, rowData[3].ToString());
                magic.description.PutSentence_EmptyStrMeansRemove(Language.Chinese, rowData[4].ToString());
                magic.description.PutSentence_EmptyStrMeansRemove(Language.Japanese, rowData[5].ToString());
                magic.abilityPrefab = CardAbility.LoadFromResources(magicFileName);
                magic.image = Resources.Load<Sprite>(General.ResourcePath.CardSprite + magicFileName);
                if (newCreated)
                {
                    AssetDatabase.CreateAsset(magic, General.ResourcePath.Absolute + General.ResourcePath.Magic + magicFileName + ".asset");
                    ++newCreatedCount;
                }
                else
                {
                    EditorUtility.SetDirty(magic);
                    ++updatedCount;
                }
            }
        }
        AssetDatabase.SaveAssets(); //存储资源
        AssetDatabase.Refresh(); //刷新
        Debug.Log("MagicAssetsCreated. updatedCount: " + updatedCount + ", newCreated: " + newCreatedCount);
    }
    private static int ToInt(string str)
    {
        if (str.Length == 0)
            return 0;
        try
        {
            return Convert.ToInt32(str);
        }
        catch(FormatException)
        {
            Debug.LogWarning(str + " is not a number.");
            return 0;
        }
    }
    private static TypeAndCountList<Substance> StrToSubstanceAndAmount(string str)
    {
        TypeAndCountList<Substance> substances = new TypeAndCountList<Substance>();
        if (str.Length == 0)
            return substances;
        bool readingAmountNumber = true;
        int amountTmp = 0;
        string lastLetter = "";
        foreach (char letter in str)
        {
            if (char.IsNumber(letter) || char.IsLower(letter) || letter == ')' || letter == '.')
            {
                lastLetter += letter;
            }
            else if (char.IsUpper(letter) || letter == '(')
            {
                if (readingAmountNumber)
                {
                    readingAmountNumber = false;
                    if (lastLetter.Length > 0)
                        amountTmp = ToInt(lastLetter);
                    else
                        amountTmp = 1;
                    lastLetter = letter.ToString();
                }
                else
                {
                    lastLetter += letter;
                }
            }
            else if (letter.Equals('+'))
            {
                if(!AvoidNull(Substance.GetByNameWithWarn(lastLetter), substance => substances.Add(substance, amountTmp)))
                    if(!unknownNames.Contains(lastLetter))
                        unknownNames.Add(lastLetter);
                readingAmountNumber = true;
                lastLetter = "";
            }
            else
            {
                Debug.Log("encounted unknown character: " + letter);
            }
        }
        if (!AvoidNull(Substance.GetByNameWithWarn(lastLetter), substance => substances.Add(substance, amountTmp)))
            if (!unknownNames.Contains(lastLetter)) 
                unknownNames.Add(lastLetter);
        return substances;
    }
    private static bool AvoidNull<T>(T element, Action<T> action)
    {
        if (element != null)
        {
            action.Invoke(element);
            return true;
        }
        return false;
    }
    private static DataSet ReadExcelFromStreamingAsset(string path)
    {
        FileStream fileStream;
        try
        {
            fileStream = File.Open(Application.streamingAssetsPath + "/" + path, FileMode.Open, FileAccess.Read);
        }
        catch (IOException)
        {
            Debug.LogError("Load Excel failed. Close any application opening the Excel file.");
            return null;
        }
        return ExcelReaderFactory.CreateOpenXmlReader(fileStream).AsDataSet();
    }
}
