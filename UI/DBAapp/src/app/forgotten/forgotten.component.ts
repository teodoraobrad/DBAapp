import { Component, OnInit } from '@angular/core';
import { SharedService } from '../shared.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-forgotten',
  templateUrl: './forgotten.component.html',
  styleUrls: ['./forgotten.component.css']
})
export class ForgottenComponent implements OnInit {

  loginForm: FormGroup;
  loginError: string | null = null;
  isLoading = false;

  constructor(private sharedService: SharedService,
    private router: Router,
    private fb: FormBuilder) { 
      this.loginForm = this.fb.group({
      korisnickoIme: ['', Validators.required],
      slazemSe: [false]  
    });
    }

  ngOnInit(): void {
  }

  reset():void{
    this.loginError = null;
    if (this.loginForm.invalid) {
      this.loginError = 'Unesite korisničko ime.';
      return;
    }
    

    const { korisnickoIme,slazemSe } = this.loginForm.value
    if (slazemSe==false) {
      this.loginError='Niste potvrdili da ste saglasni.'
      return;
    }

    this.isLoading = true;

    this.sharedService.proveriKorisnickoIme(korisnickoIme).subscribe({
      next: (zauzeto:boolean) => {
        if(zauzeto){
          
            this.sharedService.resetujLozinku(korisnickoIme).subscribe({
      next: () => {
        this.loginError = null;
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 500);
      },
      error: (err) => {
        console.error(err);
        this.loginError = 'Greška prilikom registracije. Pokušajte ponovo.';
        return;
      }
    });


        } else {this.loginError='Nalog za ovo korisnicko ime ne postoji.';
          this.isLoading=false;return;}
       
      },
      error: (err) => {
        console.error('Greška prilikom provere korisničkog imena', err);
        this.loginError='Nalog za ovo korisnicko ime ne postoji.';
        return;
      }
    });
  }

}
