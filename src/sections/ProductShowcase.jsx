import Image from "next/image";
import productImage from "@/assets/product-image.png";
import pyramidImage from "@/assets/pyramid.png";
import tubeImage from "@/assets/tube.png";
const ProductShowcase = () => {
  return (
    <section className="bg-gradient-to-b from-[#FFFFFF] to-[#D2DCFF] py-24 overflow-x-clip">
      <div className="container mx-auto px-4">
        <div className="max-w-[540px] mx-auto flex flex-col items-center text-center">
          <div className="section-heading">
            <div className="tag">Boost your productivity</div>
            <h2 className="section-title mt-5">
              A more effective way to track progress
            </h2>
            <p className="section-description mt-5">
              Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do
              eiusmod tempor incididunt ut labore et dolore magna aliqua.
            </p>
          </div>
          <div className="relative">
            <Image
              src={productImage}
              alt="Product Image"
              className="mt-5"
              width={1200}
              height={800}
            />
            <Image
              src={pyramidImage}
              alt="Pyramid Image"
              className="hidden md:block absolute -right-36 -top-32"
              hight={262}
              width={262}
            />
            <Image
              src={tubeImage}
              alt="Tube Image"
              className="hidden md:block absolute bottom-24 -left-36"
              height={248}
              width={248}
            />
          </div>
        </div>
      </div>
    </section>
  );
};

export default ProductShowcase;
