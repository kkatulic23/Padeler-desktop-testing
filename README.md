# Padeler

## Model rada na projektu
(1) Nastavak rada na projektu iz kolegija "Razvoj programskih proizvoda"

## Opis projekta
Projekt Padeler započet je na kolegiju Razvoj programskih proizvoda. Tema projekta pripada domeni sportskih i društvenih aplikacija te je usmjerena na povezivanje igrača padela radi lakšeg pronalaska partnera za igru. Cilj aplikacije je omogućiti korisnicima pronalaženje drugih igrača u blizini na temelju njihove lokacije, razine vještine i preferencija za igru.

Aplikacija je prvotno razvijena kao desktop aplikacija u razvojnom okruženju Visual Studio, korištenjem WinForms tehnologije i C# programskog jezika. Sustav omogućuje korisnicima registraciju i prijavu, izradu i uređivanje korisničkog profila, pretraživanje igrača u blizini te interakciju s drugim korisnicima.

Za razvoj projekta korištene su tehnologije .NET Framework, WinForms, C#, MySQL Server za pohranu podataka, GitHub za upravljanje verzijama koda te Visual Paradigm za modeliranje sustava.

## Projektni tim

Ime i prezime | E-mail adresa (FOI) | JMBAG | Github korisničko ime
------------  | ------------------- | ----- | ---------------------
Filip Grgac | fgrgac23@foi.hr | 0016167082 | fgrgac23
Karlo Kršak | kkrsak23@foi.hr | 0016165894 | kkrsak23
Kristian Katulić | kkatulic23@foi.hr | 0016168011 | kkatulic23

## Specifikacija projekta
Aplikacija Padeler zamišljena je kao desktop aplikacija razvijena u Visual Studiju pomoću WinForms-a u C# programskog jezika. Cilj aplikacije je povezati padel igrače na temelju njihove lokacije i razine igre. Korisnici mogu izrađivati i uređivati profile, pretraživati druge igrače u blizini, “swipeati” za odabir partnera, dopisivati se i davati ocjene nakon mečeva.

Aplikacija se sastoji od korisničkog sučelja (frontend) i poslužiteljskog (backend) dijela.
Desktop aplikacija, izrađena u Visual Studiju, zadužena je za korisničko sučelje i komunikaciju s poslužiteljem. Ona sadrži module za prijavu i registraciju korisnika, upravljanje profilom, prikaz i filtriranje igrača u blizini pomoću GPS-a, sustav za podudaranje, chat modul za komunikaciju te sustav obavijesti o novim porukama i podudaranjima.

Oznaka | Naziv | Kratki opis | Odgovorni član tima
------ | ----- | ----------- | -------------------
| F01 | Autentikacija | Omogućuje novim korisnicima kreiranje računa putem e-mail adrese. Sustav validira unesene podatke i sprema ih u bazu. Zatim se korisnici prijavljuju pomoću registriranih podataka. Sustav provjerava točnost unesenih podataka i omogućuje siguran pristup aplikaciji. | Karlo Kršak |
| F02 | Uređivanje korisničkog profila | Korisnik može ažurirati osobne podatke, dodati ili promijeniti profilnu sliku, opis, razinu vještine, lokaciju i preferencija za igru. | Kristian Katulić |
| F03 | Geolokacijsko pretraživanje | Aplikacija prikazuje druge padel igrače u blizini korisnikove lokacije, s mogućnošću podešavanja radijusa pretrage. | Karlo Kršak |
| F04 | Sustav interakcije | Korisnik može kliknuti desni gumb ako želi igrati s nekim, ili lijevi gumb za preskakanje. Ako oba igrača pritisnu desni gumb, stvara se podudaranje (match). | Filip Grgac |
| F05 | Sustav nagrađivanja korisnika | Za svaki klik korisnik će biti nagrađen sa bodom te na određenoj količini bodova će dobiti značku. | Kristian Katulić |
| F06 | Filtar za pretragu partnera | Korisnik može filtrirati potencijalne partnere po kriterijima kao što su razina vještine, spol, lokacija i dostupnost. | Karlo Kršak |
| F07 | Sustav obavijesti | Aplikacija šalje push obavijesti o novim porukama i podudaranjima. Korisnik može prilagoditi postavke obavijesti. | Filip Grgac |
| F08 | Ocjenjivanje suigrača | Nakon odigranog meča korisnik može ocijeniti partnera i ostaviti komentar. | Kristian Katulić |
| F09 | Popis ostvarenih podudaranja | Nakon ostvarenog podudaranja, korisnik i njegovi kontakt podaci će se pojaviti u popisu ostvarenih podudaranja uz mogućnost postavljanja nadimka korisnika te brisanja tog korisnika iz istog popisa. | Filip Grgac |

## Tehnologije i oprema
.NET Framework, WinForms, GitHub okruženje, Visual Paradigm, MySQL server.
