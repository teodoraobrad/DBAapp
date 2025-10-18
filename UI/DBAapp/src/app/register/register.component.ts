import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SharedService } from '../shared.service';
import { Router } from '@angular/router';
import { Korisnik } from '../models/korisnik';
import { TIM } from '../models/tim';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  timovi: TIM[] = [];
  usernameTaken = false;
  lozinkeSeNePoklapaju = false;
  uspesnoRegistrovan = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private sharedService: SharedService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      korisnickoIme: ['', [Validators.required, Validators.maxLength(20)]],
      lozinka: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]],
      confirmPassword: ['', Validators.required],
      ime: ['', [Validators.required, Validators.maxLength(15)]],
      prezime: ['', [Validators.required, Validators.maxLength(20)]],
      jmbg: ['', [Validators.pattern(/^\d{13}$/)]],
      telefon: [''],
      email: ['', [Validators.email]],
      tim: ['']
    });
  }

  ngOnInit(): void {
    //localStorage.clear();   // briše SVE iz localStorage
    //sessionStorage.clear();
    this.ucitajTimove();
  }

  ucitajTimove(): void {
    this.sharedService.getTimovi().subscribe({
      next: (data) => {
        this.timovi = data;
      },
      error: (err) => {
        console.error('Greška prilikom učitavanja timova', err);
      }
    });
  }

  proveriKorisnickoIme(): void {

    const korisnickoIme = this.registerForm.get('korisnickoIme')?.value;
    if (!korisnickoIme) return;

    this.sharedService.proveriKorisnickoIme(korisnickoIme).subscribe({
      next: (zauzeto: boolean) => {
        this.usernameTaken = zauzeto;

      },
      error: (err) => {
        console.error('Greška prilikom provere korisničkog imena', err);
      }
    });
  }
  onSubmit(): void {
    this.error = null;
    this.lozinkeSeNePoklapaju = false;
    this.uspesnoRegistrovan = false;
    this.usernameTaken = false;

    if (this.registerForm.invalid) {
      this.error = 'Molimo vas da ispravno popunite sva obavezna polja.';
      return;
    }

    const { lozinka, confirmPassword } = this.registerForm.value;
    if (lozinka != confirmPassword) {
      this.lozinkeSeNePoklapaju = true;
      return;
    }


    const korisnickoIme = this.registerForm.get('korisnickoIme')?.value;
    if (!korisnickoIme) { this.error = 'Nije uneto korisnicko ime.'; return; }

    this.sharedService.proveriKorisnickoIme(korisnickoIme).subscribe({
      next: (zauzeto: boolean) => {
        this.usernameTaken = zauzeto;

        if (this.usernameTaken) { this.error = 'Uneseno korisnicko ime je zauzeto.'; return; }
        else {
          const korisnik: Korisnik = {
            korisnickoIme: this.registerForm.value.korisnickoIme,
            lozinka: this.registerForm.value.lozinka,
            ime: this.registerForm.value.ime,
            prezime: this.registerForm.value.prezime,
            jmbg: this.registerForm.value.jmbg,
            telefon: this.registerForm.value.telefon,
            email: this.registerForm.value.email,
            tim: this.registerForm.value.tim,
            datumRegistracije: null,
            poslednjaPrijava: null,
            aktivan: false
          };

          this.sharedService.registrujKorisnika(korisnik).subscribe({
            next: () => {
              this.uspesnoRegistrovan = true;
              setTimeout(() => {
                this.router.navigate(['/login']);
              }, 2000);
            },
            error: (err) => {
              console.error(err);
              this.error = 'Greška prilikom registracije. Pokušajte ponovo.';
            }
          });
        }
      },
      error: (err) => {
        console.error('Greška prilikom provere korisničkog imena', err);
        return;
      }
    });

  }

}
