import { Component, OnInit } from '@angular/core';
import { SharedService } from '../shared.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Korisnik } from '../models/korisnik';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  loginError: string | null = null;
  isLoading = false;
  kor: Korisnik = null;
  kor1: Korisnik = null;
  dohvacen: boolean = false;
  ret: string = null;


  constructor(
    private sharedService: SharedService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      rememberMe: [false]
    });
  }

  ngOnInit(): void {
    const savedUsername = localStorage.getItem('username') || sessionStorage.getItem('username');

    if (savedUsername) {
      this.loginForm.patchValue({
        username: savedUsername,
        rememberMe: true // ovo skloni ako neces
      });

    }
  }
  async login(): Promise<void> {
    this.loginError = null;

    if (this.loginForm.invalid) {
      this.loginError = 'Unesite i korisničko ime i lozinku.';
      return;
    }

    const { username, password, rememberMe } = this.loginForm.value;

    this.isLoading = true;

    try {
      const ispravno: boolean = await firstValueFrom(this.sharedService.login(username, password));
      this.isLoading = false;
      
      if (!ispravno) {
        this.loginError = 'Pogrešno korisničko ime ili lozinka.';
        return;
      }
    
      /*if (rememberMe) {
        localStorage.setItem('username', username);
      } else {
        sessionStorage.setItem('username', username);
      }*/
      this.sharedService.loginA(username, rememberMe);
      
      this.kor = await firstValueFrom(this.sharedService.dohvatiKorisnika(username));
      
      if (this.kor.tim === 'sqladmin') {
        //alert('sql');
        sessionStorage.setItem('sqla', "true");
        this.router.navigate(['/sqladmin/intersection']).then(() => {
            // osveži stranicu da navbar odmah prikaže ime
             window.location.reload();
          });
          
      } else {
        //alert('nije sql');
sessionStorage.setItem('sqla', "false");
        try {
          const odgovorni = await firstValueFrom(this.sharedService.dohvatiOdgovornog(this.kor.tim));
          this.ret = odgovorni.korisnickoIme;
        }
        catch (err1) {
          this.ret = '';
        }

     
        if (this.kor.korisnickoIme === this.ret) {
         // alert('lead'); PROMENI NA LEAD KAD ODRADIS 
          this.router.navigate(['/user']).then(() => {
            // osveži stranicu da navbar odmah prikaže ime
             window.location.reload();
          });
          window.location.reload();
        } else {
         // alert('user');
          this.router.navigate(['/user']).then(() => {
            // osveži stranicu da navbar odmah prikaže ime
             window.location.reload();
          });
        }
      }
    } catch (err) {
      this.isLoading = false;
      this.loginError = 'Greška prilikom prijave. Pokušajte ponovo kasnije.';
      console.error(err);
    }
  }


  /*
    login(): void {
      this.loginError = null;
  
      if (this.loginForm.invalid) {
        this.loginError = 'Unesite i korisničko ime i lozinku.';
        return;
      }
  
      const { username, password, rememberMe } = this.loginForm.value
  
      this.isLoading = true;
  
      this.sharedService.login(username, password).subscribe({
    next:  (ispravno: boolean) => {
      this.isLoading = false;
  
      if (ispravno) {
        if (rememberMe) {
          localStorage.setItem('username', username);
        } else {
          sessionStorage.setItem('username', username);
        }
        
          
        
        this.kor=  this.sharedService.dohvatiKorisnika(username);
        alert(this.kor.tim)
          
        if(this.kor.tim=='sqladmin'){
          alert('sql');
                  this.router.navigate(['/sqladmin']);
                  
        }else{
            alert('nije sql');
  
         this.ret= (await this.sharedService.dohvatiOdgovornog(this.kor.tim)).korisnickoIme;
           alert(this.ret);
          if(this.kor.korisnickoIme==this.ret){
              alert('lead')
                 this.router.navigate(['/lead']);
  
          } else {   alert('user')
            this.router.navigate(['/user']);}
        }
      
      } else {
        this.loginError = 'Pogrešno korisničko ime ili lozinka.';
        return;
      }
    },
    error: (err) => {
      this.isLoading = false;
      this.loginError = 'Greška prilikom prijave. Pokušajte ponovo kasnije.';
      console.error(err);
      return;
    }
  });
    }*/
  /*dohvatiKorisnika(korisnickoIme:string):Korisnik{
     this.sharedService.dohvatiKorisnika(korisnickoIme).subscribe({
      next: (kor:Korisnik) => {
        this.dohvacen=true;
        return kor;
    },error:(err)=>{
      this.isLoading = false;
    this.loginError = 'Greška prilikom provere korisnika. Pokušajte ponovo kasnije.';
    console.error(err);
    this.dohvacen=true;
    return;
    }
    
  });return null;
  }
  dohvatiOdgovornog(imeTima:string):Korisnik{
    this.sharedService.dohvatiOdgovornog(imeTima).subscribe({
      next: (kor:Korisnik) => {
        this.dohvacen=true;
        return kor;
    },error:(err)=>{
      this.isLoading = false;
    this.loginError = 'Greška prilikom provere korisnika. Pokušajte ponovo kasnije.';
    console.error(err);
    this.dohvacen=true;
    return;
    }
    
  });
  return null;
  }
*/


}
