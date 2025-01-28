import acmeLogo from "../assets/logo-acme.png";
import quantumLogo from "../assets/logo-quantum.png";
import echoLogo from "../assets/logo-echo.png";
import celestialLogo from "../assets/logo-celestial.png";
import pulseLogo from "../assets/logo-pulse.png";
import apexLogo from "../assets/logo-apex.png";
import Image from "next/image";

export default function LogoTicker() {
  return (
    <div className="bg-white py-8 md:py-12">
      <div className="container mx-auto px-4">
        <div className="flex items-center justify-center overflow-hidden masked-gradient">
          <div className="flex gap-14 flex-none">
            <Image
              className="logo-ticker-image"
              src={acmeLogo}
              alt="Acme Logo"
              width={100}
              height={100}
            />
            <Image
              className="logo-ticker-image"
              src={quantumLogo}
              alt="Quantum Logo"
              width={100}
              height={100}
            />
            <Image
              className="logo-ticker-image"
              src={echoLogo}
              alt="Echo Logo"
              width={100}
              height={100}
            />
            <Image
              className="logo-ticker-image"
              src={celestialLogo}
              alt="Celestial Logo"
              width={100}
              height={100}
            />
            <Image
              className="logo-ticker-image"
              src={pulseLogo}
              alt="Pulse Logo"
              width={100}
              height={100}
            />
            <Image
              className="logo-ticker-image"
              src={apexLogo}
              alt="Apex Logo"
              width={100}
              height={100}
            />
          </div>
        </div>
      </div>
    </div>
  );
}
