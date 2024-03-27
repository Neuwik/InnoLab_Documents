using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    [SerializeField]
    private Image[] health;
    public Sprite FullHearts;
    public Sprite EmptyHearts;
    public Sprite ArmoredHeart;
    public Sprite One;
    public Sprite Zero;

    private int maxHPBinaryLength;
    public int tmpHealth;

    [SerializeField]
    private bool binaryNumber = false;

    private void Start()
    {
        currentHealth = maxHealth;
        maxHPBinaryLength = Convert.ToString(currentHealth, 2).Length;
        UpdateHealthUI();
    }

    public override void TakeDamage(int amount)
    {
        if (tmpHealth > 0)
        {
            tmpHealth -= amount;
            if (tmpHealth < 0)
            {
                currentHealth += tmpHealth;
                tmpHealth = 0;
            }
        }
        else
        {
            base.TakeDamage(amount);
        }

        UpdateHealthUI();
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        UpdateHealthUI();
    }

    public void GetShield(int amount)
    {
        tmpHealth += amount;
        if (tmpHealth >= maxHealth)
        {
            tmpHealth = maxHealth;
        }
        UpdateHealthUI();
    }

    //Testing
    /*public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.tag == "HealOrb")
            Heal(1);
        else if (collision.gameObject.tag == "DamageOrb")
            TakeDamage(1);
        else if (collision.gameObject.tag == "ArmoreOrb")
            GetShield(1);
    }*/

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("Trigger detected with: " + collision.gameObject.name);
        

        if (collision.gameObject.tag == "HealOrb")
        {
            Heal(1);
            collision.gameObject.SetActive(false);
        }

        else if (collision.gameObject.tag == "DamageOrb")
            TakeDamage(1);

        else if (collision.gameObject.tag == "ArmoreOrb")
        {
            GetShield(1);
            collision.gameObject.SetActive(false);
        }
    }

    private void UpdateHealthUI()
    {
        if (binaryNumber)
        {
            UpdateHealthUIBinary();
        }
        else
        {
            UpdateHealthUIDigits();
        }
    }

    private void UpdateHealthUIDigits()
    {
        for (int i = 0; i < health.Length; i++)
        {
            if (i < currentHealth)
            {
                health[i].sprite = One;
            }
            else
            {
                health[i].sprite = Zero;
            }

            if (i < maxHealth)
            {
                health[i].enabled = true;
            }
            else
            {
                health[i].enabled = false;
            }
        }
    }

    private void UpdateHealthUIBinary()
    {
        string binaryStr = Convert.ToString(currentHealth, 2);
        while (binaryStr.Length < maxHPBinaryLength)
        {
            binaryStr = '0' + binaryStr;
        }
        char[] binary = binaryStr.ToCharArray();
        for (int i = 0; i < health.Length; i++)
        {
            if (i >= maxHPBinaryLength)
            {
                health[i].enabled = false;
                continue;
            }

            if (binary[i] == '1')
            {
                health[i].sprite = One;
            }
            else
            {
                health[i].sprite = Zero;
            }
        }
    }

    private void UpdateHealthUI_Hearts()
    {
        for (int i = 0; i < health.Length; i++)
        {
            if (i < tmpHealth)
            {
                health[i].sprite = ArmoredHeart;
            }
            else if (i < currentHealth)
            {
                health[i].sprite = FullHearts;
            }
            else
            {
                health[i].sprite = EmptyHearts;
            }

            if (i < maxHealth)
            {
                health[i].enabled = true;
            }
            else
            {
                health[i].enabled = false;
            }
        }
    }
}
