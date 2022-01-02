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

# Part 2

Password hashes were taken from Pavel in security chat. You can find them in part2_data folder
Passwords in weakHash were hashed using MD5, and in strongHash using argon2i.

## MD5
We used the hashcat utility and a dictionary of the 100k most common passwords from previous part.
Command:

.\hashcat.exe -m 0 '.\weakHash.csv' .\100k_passwords.txt -o md5_hacked.txt

-m 0 - means md5 mode. I just asked Pavel what type of hashing he used
'.\weakHash.csv' - file with hash we want to hack
.\100k_passwords.txt - dictionary with passwords
-o md5_hacked.txt - output file

Output:

![1](https://user-images.githubusercontent.com/42899572/147879293-3fc2d1f3-45c8-47ae-bd34-7104105fab33.jpg)

![3](https://user-images.githubusercontent.com/42899572/147879803-b7e3270f-9015-4abf-8f12-d2a2508c7eb4.jpg)

So 72,721 passwords out of 100,000 were picked in 2 minutes. This indicates the weakness of the MD5 hashing algorithm. Result passwords yoc can see in md5_hacked.txt

## argon2i

Hashcat is not suitable for this task, so we used John the Ripper
Command:

.\run\john.exe --format=argon2 '.\strongHash.csv' --wordlist=100k_passwords.txt

--format=argon2 - means argon2 mode. I get this from hash structure
'.\strongHash.csv' - file with hash we want to hack
--wordlist=100k_passwords.txt - specify to use our dictionary with passwords

Output:

![2](https://user-images.githubusercontent.com/42899572/147879292-a9a199a6-f8a5-4887-82d5-aada9c449b44.jpg)

So 31 passwords out of 100,000 were picked in almost 6 minutes. Only few, most weak passwords, the results are outstanding. 
The reason for this is because we can't use GPU for cracking argon. 
Hacked passwords yoc can see on screenshot and in argon_hacked.txt