import Image from "next/image";
import { ArrowRight } from "lucide-react";
import { Menu } from "lucide-react";
import Logo from "@/assets/logosaas.png";

function Header() {
  return (
    <header className="sticky top-0 z-50 backdrop-blur-md z-20">
      <div className="flex justify-center items-center py-3 gap-3 bg-black text-sm text-white">
        <div className="inline-flex gap-1 items-center">
          <p className="text-white/60 hidden md:block">
            Tu próxima aventura te espera
          </p>
          <p>¡Reserva con facilidad!</p>
          <ArrowRight className="w-4 h-4" />
        </div>
      </div>
      <div className="py-5">
        <div className="container mx-auto px-4">
          <div className="flex justify-between items-center">
            <Image src={Logo} alt="Logo" width={40} height={40} />
            <Menu className=" h-5 w-5 md:hidden" />
            <nav className="hidden md:flex gap-6 text-black/60 items-center">
              <a href="#">Nosotros</a>
              <a href="#">Servicios</a>
              <a href="#">Clientes</a>
              <a href="#">Contacto</a>
              <button className="btn btn-primary">Reserva</button>
            </nav>
          </div>
        </div>
      </div>
    </header>
  );
}

export default Header;
