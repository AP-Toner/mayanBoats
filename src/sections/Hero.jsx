import Image from "next/image";
import { ArrowRight } from "lucide-react";
import cogImage from "@/assets/cog.png";
import hill1 from "@/assets/hill1.png";
import hill2 from "@/assets/hill2.png";
import hill3 from "@/assets/hill3.png";
import hill4 from "@/assets/hill4.png";
import hill5 from "@/assets/hill5.png";
import leaf from "@/assets/leaf.png";
import plant from "@/assets/plant.png";
import tree from "@/assets/tree.png";
import cylinderImage from "@/assets/cylinder.png";
import noodle from "@/assets/noodle.png";

const Hero = () => {
  return (
    //bg-[radial-gradient(ellipse_200%_100%_at_bottom_left,#183EC2,#EAEEFE_100%)]
    <section className="overflow-x-clip pt-8 md:pt-5 md:pb-10 pb-20 relative">
      <div className="absolute top-0 left-0 w-full h-full z-[-1]">
        <Image
          src={hill1}
          alt="Hill 1"
          className="absolute md:h-full top-0 left-0 w-full object-cover"
        />
        <Image
          src={hill2}
          alt="Hill 2"
          className="absolute md:h-full top-0 left-0 w-full object-cover"
        />
        <Image
          src={hill3}
          alt="Hill 3"
          className="absolute md:h-full top-0 left-0 w-full object-cover"
        />
        <Image
          src={hill4}
          alt="Hill 4"
          className="absolute md:h-full top-0 left-0 w-full object-cover"
        />
        <Image
          src={hill5}
          alt="Hill 5"
          className="absolute md:h-full top-0 left-0 w-full object-cover"
        />
        <Image
          src={leaf}
          alt="Leaf"
          className="absolute md:h-full top-0 left-0 w-full object-cover"
        />
        <Image
          src={plant}
          alt="Plant"
          className="absolute md:h-full top-0 left-0 w-full object-cover"
        />
        <Image
          src={tree}
          alt="Tree"
          className="absolute md:h-full top-0 left-0 w-full object-cover"
        />
      </div>

      <div className="container mx-auto px-4 max-w-screen-xl relative">
        <div className="md:flex items-center justify-between">
          <div className="md:w-[478px] text-center md:text-left">
            <div className="tag">Version 2.0 is here</div>
            <h1 className="text-5xl md:text-7xl font-bold tracking-tighter bg-gradient-to-b from-black to-[#001E80] text-transparent bg-clip-text mt-6">
              ¡Haz tu reserva hoy mismo!
            </h1>
            <p className="text-xl text-[#010D3E] tracking-tight mt-6">
              ¡Embárcate en la experiencia de tu vida! Reserva tu yate ahora y
              disfruta de un día inolvidable en el mar. Relájate, explora y vive
              la aventura con el mejor servicio y comodidad. ¡Tu próxima
              escapada está solo a un clic de distancia!
            </p>
            <div className="flex gap-1 items-center mt-[30px]">
              <button className="btn btn-primary">¡Reserva ahora!</button>
              <button className="btn btn-text gap-1">
                <span>Ver más</span>
                <ArrowRight className="h-4 w-4" />
              </button>
            </div>
          </div>

          <div className="mt-20 md:mt-0 md:h-[648px] md:flex-1 relative">
            <Image
              src={cogImage}
              alt="cog Image"
              className="md:absolute md:h-full md:w-auto md:max-w-none md:-left-6 lg:left-0"
            />
            <Image
              src={cylinderImage}
              alt="cylinder"
              width={220}
              height={220}
              className="hidden md:block -top-8 -left-32 md:absolute"
            />
            <Image
              src={noodle}
              alt="noodle"
              width={220}
              height={220}
              className="hidden absolute lg:block top-[524px] left-[448px] rotate-[30deg]"
            />
          </div>
        </div>
      </div>
    </section>
  );
};

export default Hero;
