# Art Studio Edit

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/X8X8Z15EK)

Allows you to encode image files as Animal Jam art files, or decode AJ art files into image files.

## How To Use

### Fetch Your UUID
- Use the "Log In" button to search your AJ Classic files for your encoded UUID.
  > **Note:** The mechanics for logging the UUID in versions 1.1 - 1.3 no longer work.

### Encoding
- You may choose from `.bmp`, `.jpg`, or `.png` file formats.
  > *though* I recommend using .jpg or .bmp, as .png can experience encoding issues and may not be loadable inside AJ Art Studio. Additionally, watch out for large file sizes. I recommend exporting the image with 95% quality.

#### Regular Art
- For regular art, I recommend the image dimensions to be 711 pixels wide and 434 pixels high (the size of the Art Studio canvas).

#### Pixel Art
- For pixel art, the initial format should be 51 pixels x 31 pixels when you edit the image. Before the final import, scale it by a factor of 14 (714x434) using hard edges (Nearest Neighbor) to prevent the edges from being softened when scaled.

 > **Note:** When you encode an `.ajart` file with a specific UUID, it will only load on that account in AJ.

### Decoding
- You may choose an `.ajart` or `.ajgart` file. When decoding, ensure the UUID corresponds with the account that originally created the artwork.

## Help

### Masterpieces Disabled

On some images you import, you may get a warning saying masterpieces are disabled on the account when you go to create it. If this happens, masterpieces aren't actually disabled; it's just that the image has too much quality. To bypass this, you need to put a solid color on the top left corner to trick the system into thinking it's lower quality. You can place a small dot with the AJ paint tool, and it won't be seen through the frame when you create the masterpiece.

## Warning

**Using this software can lead to your account being banned.** Use this app responsibly and at your own risk.

- **Art Content:** The Animal Jam community and developers have strict guidelines on the content of the art that can be uploaded. Uploading inappropriate or offensive artwork can result in your account being banned.
- **Account Security:** Ensure you are using the software for its intended purpose and that you are aware of the consequences. Misuse of this software to bypass AJ's art submission rules or to upload unauthorized content can lead to the permanent closure of your account.
- **Developer Responsibility:** The developers of this software are not responsible for any actions taken against your account due to the misuse of this tool. Always adhere to the community guidelines set by Animal Jam.

By using this software, you acknowledge that you understand the risks and accept full responsibility for any consequences resulting from its use. That being said, I've yet to be banned by uploading art.

## Note

**Always practice mindful internet habits.** This app runs on your computer and utilizes your file content in order to grab your UUID and use it to encode art. Regardless of your trust in me, or lack thereof, I highly encourage you to review the source code thoroughly to ensure it aligns with your expectations and does not pose any security risks to yourself.

## Legal Stuff
In the U.S., Section 103(f) of the Digital Millennium Copyright Act (DMCA) (17 USC § 1201 (f) - Reverse Engineering) states that it is legal to reverse engineer and circumvent protections to achieve interoperability between computer programs (such as information transfer between applications). https://www.eff.org/issues/coders/reverse-engineering-faq

1. Notwithstanding the provisions of subsection (a)(1)(A), a person who has lawfully obtained the right to use a copy of a computer program may circumvent a technological measure that effectively controls access to a particular portion of that program for the sole purpose of identifying and analyzing those elements of the program that are necessary to achieve **interoperability** of an independently created computer program with other programs, and that have not previously been readily available to the person engaging in the circumvention, to the extent any such acts of identification and analysis do not constitute infringement under this title.

2. Notwithstanding the provisions of subsections (a)(2) and (b), a person may develop and employ technological means to circumvent a technological measure, or to circumvent protection afforded by a technological measure, in order to enable the identification and analysis under paragraph (1), or for the purpose of enabling interoperability of an independently created computer program with other programs, if such means are necessary to achieve such interoperability, to the extent that doing so does not constitute infringement under this title.

3. The information acquired through the acts permitted under paragraph (1), and the means permitted under paragraph (2), may be made available to others if the person referred to in paragraph (1) or (2), as the case may be, provides such information or means solely for the purpose of enabling interoperability of an independently created computer program with other programs, and to the extent that doing so does not constitute infringement under this title or violate applicable law other than this section.

4. For purposes of this subsection, the term ‘interoperability’ means the ability of computer programs to exchange information, and of such programs mutually to use the information which has been exchanged.

The Copyright Act of 1976, 17 U.S.C. § 107 states specifically:

Notwithstanding the provisions of sections 106 and 106A, the fair use of a copyrighted work, including such use by reproduction in copies or phonorecords or by any other means specified by that section, for purposes such as **criticism**, **comment**, news reporting, **teaching (including multiple copies for classroom use)**, scholarship, or **research**, is not an infringement of copyright. In determining whether the use made of a work in any particular case is a fair use the factors to be considered shall include—

1. The purpose and character of the use, including whether such use is of a commercial nature or is for nonprofit educational purposes;
2. The nature of the copyrighted work;
3. The amount and substantiality of the portion used in relation to the copyrighted work as a whole; and
4. The effect of the use upon the potential market for or value of the copyrighted work.

**The fact that a work is unpublished shall not itself bar a finding of fair use if such finding is made upon consideration of all the above factors.**

## Credits

> -v31l-sys and cfr0st for reverse engineering Animal Jam and creating Art Studio Edit v1.1 - 1.3

> -EddieTristes for fixing the UUID grabber to work with the AJ Classic app, updating UI, and updating Art Studio Edit
