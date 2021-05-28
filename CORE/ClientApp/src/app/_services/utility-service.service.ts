import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UtilityService {

  constructor() { }

  validateEmail(email: string): Boolean {
    const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
  }

  validatePassword(password: string): Array<string>{
    var errors: Array<string> = [];
    var specialCharacters = /[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/;
    if (password.length<8){
      errors.push("The password given is too short.")
    }
    if (!/[a-z]/.test(password)){
      errors.push("The password given does not have a lower case letter.")
    }
    if (!/[A-Z]/.test(password)){
      errors.push("The password given does not have an upper case letter.")
    }
    if (!/\d/.test(password)){
      errors.push("The password given does not have a number.")
    }
    if (!specialCharacters.test(password)){
      errors.push("The password given does not have a special character.")
    }
    return errors
  }

  compareObject(object1, object2): boolean {
    const keys1 = Object.keys(object1);
    const keys2 = Object.keys(object2);
  
    if (keys1.length !== keys2.length) {
      return false;
    }
  
    for (const key of keys1) {
      const val1 = object1[key];
      const val2 = object2[key];
      const areObjects = this.isObject(val1) && this.isObject(val2);
      if (
        areObjects && !this.compareObject(val1, val2) ||
        !areObjects && val1 !== val2
      ) {
        return false;
      }
    }
  
    return true;
  }
  
  private isObject(object): boolean {
    return object != null && typeof object === 'object';
  }
}
