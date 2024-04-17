import { Component, OnDestroy, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { LayoutService } from 'src/app/layout/service/app.layout.service';
import { ACCESS_TOKEN, REFRESH_TOKEN } from 'src/app/shared/constants/key.contants';
import { LoginRequestDto } from 'src/app/shared/models/Login-request.dto';
import { LoginResponseDto } from 'src/app/shared/models/Login-response.dto';
import { AuthService } from 'src/app/shared/services/auth.service';
import { TokenStorageService } from 'src/app/shared/services/token.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: [
    `
      :host ::ng-deep .pi-eye,
      :host ::ng-deep .pi-eye-slash {
        transform: scale(1.6);
        margin-right: 1rem;
        color: var(--primary-color) !important;
      }
    `,
  ],
})
export class LoginComponent implements OnDestroy {
  private ngUnSubcribe = new Subject<void>();
  valCheck: string[] = ['remember'];
  blockedPanel: boolean = false;
  password!: string;

  loginForm: FormGroup;
  //   private authServie = inject(AuthService);
  constructor(
    public layoutService: LayoutService,
    private fb: FormBuilder,
    private route: Router,
    private authServie: AuthService,
    private tokenService: TokenStorageService
  ) {
    this.loginForm = this.fb.group({
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    });
  }

  login() {
    this.toggleBlockUI(true);
    var request: LoginRequestDto = {
      username: this.loginForm.controls['username'].value,
      password: this.loginForm.controls['password'].value,
    };

    this.authServie
      .login(request)
      .pipe(takeUntil(this.ngUnSubcribe))
      .subscribe({
        next: (res: LoginResponseDto) => {
          this.tokenService.saveToken(res.access_token);
          this.tokenService.saveRefreshToken(res.refresh_token);
          this.toggleBlockUI(false);
          this.route.navigate(['']);
        },
        error: err => {
          alert('sai tai khoan hoac mat khau');
          this.toggleBlockUI(false);
        },
      });
  }

  toggleBlockUI(enable: boolean) {
    if (enable == true) {
      this.blockedPanel = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
      }, 1000);
    }
  }

  ngOnDestroy(): void {
    this.ngUnSubcribe.next();
    this.ngUnSubcribe.complete();
  }
}
