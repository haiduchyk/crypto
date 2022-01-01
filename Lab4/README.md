# Part 1

Created 100,000 passwords, and hashed them using MD5, SHA1 and BCrypt. 
You can find results in generated-sha1.csv, generated-md5.csv and generated-bcrypt.csv

## How top 100 common was generated
Take randomly from 100k_passwords.txt with 10% chance.
Take randomly from top100_common_passwords.txt
Top 100 common passwords got from https://nordpass.com/most-common-passwords-list/

## How top 100k passwords was generated
Take randomly from 100k_passwords.txt with 77% chance.
100k passwords got from https://github.com/danielmiessler/SecLists/blob/master/Passwords/Common-Credentials/100k-most-used-passwords-NCSC.txt

## How random passwords was generated
Choose random length between 8 and 16. 
And then fill with characters from "abcdefghijklmnopqrstuvwxyz123456789!\"#$%&\'()*+,-./:;<=>?@[\\]^_`{|}~". Generate with 3% chance.

## How human alike passwords was generated
Select one of the 100k passwords, choose few random modificators, and apply them one buy one

AddNumbersInEndModificator - 0.5
AddCharacterModificator - 0.5
ReplaceCharacterModificator - 0.1
ReverseModificator - 0.05
LowerCaseModificator - 0.1
UpperCaseModificator - 0.2

Generate with 10% chance.