export class Configuration {
  ministryName?: string;
  index?: string;
  about?: string;
  beliefs?: string;
  services?: string;
  logo?: Blob;

  public hasMinistryName(): boolean {
    return this.ministryName !== undefined && this.ministryName.length > 0;
  }

  public getMinistryName(): string {
    return this.hasMinistryName() ? '' + this.ministryName : '';
  }

  public hasIndex(): boolean {
    return this.index !== undefined;
  }

  public hasAbout(): boolean {
    return this.about !== undefined;
  }

  public hasBeliefs(): boolean {
    return this.beliefs !== undefined;
  }

  public hasServices(): boolean {
    return this.services !== undefined;
  }

  public hasLogo(): boolean {
    return this.logo !== undefined;
  }

  public getIndex(): string {
    return this.index !== undefined ? this.index : '';
  }

  public getAbout(): string {
    return this.about !== undefined ? this.about : '';
  }

  public getBeliefs(): string {
    return this.beliefs !== undefined ? this.beliefs : '';
  }

  public getServices(): string {
    return this.services !== undefined ? this.services : '';
  }

  public getLogo(): Blob | undefined {
    return this.hasLogo() ? this.logo : undefined;
  }
}
