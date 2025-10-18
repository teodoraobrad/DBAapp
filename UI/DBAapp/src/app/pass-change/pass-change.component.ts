import { Component, OnInit } from '@angular/core';
import { SharedService } from '../shared.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-pass-change',
  templateUrl: './pass-change.component.html',
  styleUrls: ['./pass-change.component.css']
})
export class PassChangeComponent implements OnInit {

  loginForm: FormGroup;
  loginError: string | null = null;
  isLoading = false;

  constructor(
    private sharedService: SharedService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      oldpassword: ['', Validators.required],
      newpassword: ['', Validators.required] 
    });
  }

  ngOnInit(): void {
     
  }

  passChange(): void {
    this.loginError = null;

    if (this.loginForm.invalid) {
      this.loginError = 'Unesite korisničko ime, trenutnu i novu lozinku.';
      return;
    }

    const { username, oldpassword, newpassword } = this.loginForm.value

    this.isLoading = true;

    if(oldpassword==newpassword){
      this.loginError='Nema promene u lozinci.'
      return;
    }

    this.sharedService.login(username, oldpassword).subscribe({
    next: (ispravno: boolean) => {
    this.isLoading = false;

    if (ispravno) {
      
      this.sharedService.passChange(username,newpassword).subscribe(
        {
          next:(rez:string)=>{
            
            alert('Lozinka je uspesno promenjena. Bicete preusmereni na stranicu za prijavu')
      this.router.navigate(['/login']);

          }, error:(err)=>{
            this.isLoading = false;
            this.loginError = 'Greška prilikom prijave. Pokušajte ponovo kasnije.';
            console.error(err);
          }
        }
      );


    } else {
      this.loginError = 'Pogrešno korisničko ime ili lozinka.';
    }
  },
  error: (err) => {
    this.isLoading = false;
    this.loginError = 'Greška prilikom prijave. Pokušajte ponovo kasnije.';
    console.error(err);
  }
});
  }
}
