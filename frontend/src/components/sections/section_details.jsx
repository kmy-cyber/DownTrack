import { useParams, useNavigate } from "react-router-dom";
import { Card, CardHeader, CardBody, Typography } from "@material-tailwind/react";
import { ArrowUpRightIcon } from "@heroicons/react/24/solid";
import sectionData from "@/data/sections-data";
import departmentData from "@/data/department-data";
import userListData  from "@/data/users-table-data";

export function SectionDetails() {
  const { sectionId } = useParams();
  const navigate = useNavigate();

  // To Do: Use real data from backend
  const section = sectionData.find((sect) => sect.id === sectionId);
  const departments = departmentData.filter((dept) => dept.sectionId === sectionId);
  const sectionManager = userListData.find((user) => user.role === "section_manager" && user.sectionId === section.id)
  console.log(sectionManager);
  
  if (!section) {
    return <Typography color="red">Section not found!</Typography>;
  }

  const handleDepartmentClick = (departmentId) => {
    navigate(`/dashboard/director/sections/${sectionId}/departments/${departmentId}`);
  };

  return (
    <div className="mt-12 mb-8 flex flex-col gap-12">
      <Card>
        <CardHeader variant="gradient" color="gray" className="mb-4 p-6">
          <Typography variant="h6" color="white">
            Section Details: {section.name}
          </Typography>
        </CardHeader>
        <CardBody>
          <Typography className="mb-4 text-blue-gray-600">{section.description}</Typography>
          
          {sectionManager ? (
            <Typography variant="h6" className="mb-4 text-blue-gray-700">
              Section Manager: {sectionManager.name} ({sectionManager.username})
            </Typography>
          ):(
            <Typography variant="h6" className="mb-4 text-blue-gray-700">
             No section manager assigned!
            </Typography>
          )}

          <Typography variant="h6" className="mb-4 text-blue-gray-700">
            Departments in this Section:
          </Typography>
          <table className="w-full table-auto border-collapse border border-gray-200">
            <thead>
              <tr>
                <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">
                  Department Name
                </th>
                <th className="border border-gray-200 px-4 py-2 bg-gray-50 text-gray-700">
                  Description
                </th>
              </tr>
            </thead>
            <tbody>
              {departments.map((dept) => (
                <tr
                  key={dept.id}
                  className="hover:bg-gray-50 cursor-pointer"
                  onClick={() => handleDepartmentClick(dept.id)}
                >
                  <td className="border border-gray-200 px-4 py-2 flex items-center gap-1">
                    {dept.name}
                    <ArrowUpRightIcon className="w-4 h-4 text-blue-500"/>
                  </td>
                  <td className="border border-gray-200 px-4 py-2">{dept.description}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </CardBody>
      </Card>
    </div>
  );
}

export default SectionDetails;