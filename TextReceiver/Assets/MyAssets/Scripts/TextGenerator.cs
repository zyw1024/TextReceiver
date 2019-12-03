using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TextGenerator : MonoBehaviour
{
    public string userName;
    public int participantID;
    public string experimentCondition;

    public Text messageOnBoard;
    public string generatedMessage;
    System.Random rand = new System.Random();
    public int randomInterval;

    public string savingDirectory;

    public int currentTimeInSeconds;

    //public AudioClip audioClip;
    public AudioSource audioSource;


    void Start()
    {
        // Test user
        userName = "Yiwen";
        participantID = 1;
        experimentCondition = "Text Only";

        // Initialize the time interval for generating the first message
        randomInterval = rand.Next(20, 40);
        currentTimeInSeconds = -1;
        InvokeRepeating("GenerateMessageAtRandomTime", 0, 1);
        
        // Set the directory for saving raw messages.
        savingDirectory = Application.persistentDataPath;

        //Debug.Log(savingDirectory);
        if (!Directory.Exists(savingDirectory))
        {
            Directory.CreateDirectory(savingDirectory);
        }
    }

    
    // Generate message and save the message to a txt file
    public string MessageGenerate()
    {
        int randomDigit = rand.Next(1000000000, 1580000000);
        WriteFileByLine(savingDirectory, userName + "_RawText" + ".txt", randomDigit.ToString());
        
        return randomDigit.ToString();
    }

    public bool GenerateMessageAtRandomTime()
    {
        currentTimeInSeconds++;
        bool isMessageGenerated = false;
        if (currentTimeInSeconds != 0 && currentTimeInSeconds % randomInterval == 0)
        {

            this.generatedMessage = MessageGenerate();

            // Reset the time interval for next message generation
            randomInterval = rand.Next(20, 40);
            currentTimeInSeconds = 0;
            isMessageGenerated = true;
            
            //Debug.Log("Message generate at " + currentTimeInSeconds.ToString());
            messageOnBoard.text = generatedMessage;
            
            // Play the message tone when the message arrives
            audioSource.Play();
        }
        return isMessageGenerated;
    }

    public void WriteFileByLine(string directory_path, string file_name, string str_info)
    {
        StreamWriter sw;

        if (!System.IO.Directory.Exists(directory_path))
            System.IO.Directory.CreateDirectory(directory_path);
        if (!File.Exists(directory_path + "//" + file_name))
        {
            // Create a file for text
            sw = File.CreateText(directory_path + "//" + file_name);
            //Debug.Log("Experiment record created successfully！");
        }
        else
        {
            // Open the remaining file to append
            sw = File.AppendText(directory_path + "//" + file_name);
        }

        // Write line by line
        sw.WriteLine(str_info);
        sw.WriteLine("\r\n");

        //release the stream
        sw.Close();
        sw.Dispose();
    }

}
